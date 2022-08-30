using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HeroAgent : Agent
{
    private Animator anim;

    public GameObject zombiePrefab;
    public GameObject citizenPrefab;
    public GameObject safetyZoneGo;
    public GameObject groundGo;
    private Coroutine moveRoutine;

    private Renderer groundRenderer;
    public Material redMat;
    public Material greenMat;
    public Material groundMat;

    private bool isMove;

    private void Start()
    {
        isMove = false;
        this.anim = this.GetComponentInChildren<Animator>();
        this.groundRenderer = this.groundGo.GetComponent<Renderer>();
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
            if (this.GetCumulativeReward() <= this.GetCumulativeReward() + 0.9)
            {
                AddReward(-0.01f);
            }
            this.anim.SetInteger("State", 1);
        }
        else
        {
            this.anim.SetInteger("State", 0);
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
        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);

        Physics.Raycast(transform.position + new Vector3(0, 4, 0), transform.forward, out hit, 5f);

        if (hit.collider == null && this.GetCumulativeReward() <= this.GetCumulativeReward() + 0.9f)
        {
            AddReward(-0.01f);
        }

        if (hit.collider != null
            && (hit.collider.CompareTag("zombie") || hit.collider.CompareTag("citizen")))
        {
            Destroy(hit.collider.gameObject);

            if (hit.collider.CompareTag("zombie"))
            {
                Destroy(hit.collider.gameObject);
                StartCoroutine(this.ChangeGroundColor(this.greenMat));
                AddReward(1f);
            }

            if (hit.collider.transform.CompareTag("citizen"))
            {
                Destroy(hit.collider.gameObject);
                StartCoroutine(this.ChangeGroundColor(this.redMat));
                AddReward(0.5f);
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

        yield return new WaitForSeconds(0.2f);
        //yield return null;
        moveRoutine = null;
        isMove = false;
    }
    private IEnumerator ChangeGroundColor(Material mat)
    {
        this.groundRenderer.material = mat;
        yield return new WaitForSeconds(0.3f);
        this.groundRenderer.GetComponent<Renderer>().material = this.groundMat;
    }
}
