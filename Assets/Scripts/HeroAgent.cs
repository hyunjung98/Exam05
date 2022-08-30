using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HeroAgent : Agent
{
    public GameObject zombiePrefab;
    public GameObject citizenPrefab;
    public GameObject safetyZoneGo;
    private Coroutine moveRoutine;

    private bool isMove;

    private void Start()
    {
        isMove = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition.x);
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(1, 0, -2);
        this.safetyZoneGo.transform.localPosition = new Vector3(0, 0, 0);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discreteActions = actions.DiscreteActions;

        var h = discreteActions[0] - 1;

        //Debug.Log("isMove : " + isMove);

        if ((h == 1 || h == -1) && isMove == false)
        {
            this.Move(h);
        }

        var isShot = discreteActions[1];

        if (isShot == 1)
        {
            this.ShootGun();
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var h = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            h = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            h = 2;
        }

        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = h;

        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[1] = 1;
        }
        else
        {
            discreteActions[1] = 0;
        }
    }

    public void ShootGun()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position + new Vector3(0, 4, 0), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 7f, Color.red, 1f);

        Physics.Raycast(transform.position + new Vector3(0, 4, 0), transform.forward, out hit, 7f);

        if (hit.collider != null
            && (hit.collider.CompareTag("zombie") || hit.collider.CompareTag("citizen")))
        {
            Destroy(hit.collider.gameObject);
            AddReward(0.001f);

            if (hit.collider.CompareTag("zombie"))
            {
                Destroy(hit.collider.gameObject);
                AddReward(1f);
            }

            if (hit.collider.transform.CompareTag("citizen"))
            {
                Destroy(hit.collider.gameObject);
                EndEpisode();
            }
        }
    }

    public void Move(int h)
    {
        if (this.transform.localPosition.x <= -3f && h == -1)
            return;
        if (this.transform.localPosition.x >= 3f && h == 1)
            return;

        if (moveRoutine == null)
            this.moveRoutine = StartCoroutine(MoveRoutine(h));
    }

    private IEnumerator MoveRoutine(int h)
    {
        isMove = true;
        Vector3 dir = this.transform.localPosition;
        if (h == -1)
        {
            dir.x -= 2f;
        }
        else if(h == 1)
        {
            dir.x += 2f;
        }

        this.transform.localPosition = dir;

        //yield return new WaitForSeconds(0.5f);
        yield return null;
        moveRoutine = null;
        isMove = false;
    }
}
