using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public GameObject[] arrTarget;
    public GameObject[] pointGo;

    public UnityAction spawnComplete;
    private int spawnCount;

    public void Init()
    {
        spawnCount = 0;
    }


    private void Start()
    {
        this.CreateTarget();
        this.Init();
    }

    public void CreateTarget()
    {
        this.StartCoroutine(this.CreateRoutine());
    }

    private IEnumerator CreateRoutine()
    {
        //while (true)
        //{
        //    int randPointIndex = Random.Range(0, 4);

        //    int randTargetIndex = Random.Range(0, 2);
        //    var targetGo = Instantiate<GameObject>(this.arrTarget[randTargetIndex]);
        //    targetGo.transform.SetParent(this.transform);

        //    //var randPoint =  this.pointGo.transform.GetChild(randPointIndex).position;
        //    var randPoint = this.pointGo[randPointIndex].transform.position;

        //    targetGo.transform.position = randPoint;
        //    yield return new WaitForSeconds(1f);
        //    spawnCount++;
        //    if (spawnCount >= 9)
        //        break;
        //}
        //this.spawnComplete();

        while (true)
        {
            int randPointIndex = Random.Range(0, 4);

            int randTargetIndex = Random.Range(0, 2);
            var targetGo = Instantiate<GameObject>(this.arrTarget[randTargetIndex]);

            //var randPoint =  this.pointGo.transform.GetChild(randPointIndex).position;
            var randPoint = this.pointGo[randPointIndex].transform.position;
            targetGo.transform.parent = this.transform;
            targetGo.transform.position = randPoint;
            yield return new WaitForSeconds(2f);
        }

    }
}
