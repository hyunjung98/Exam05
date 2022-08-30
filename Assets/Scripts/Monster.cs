using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;

    void Update()
    {
        this.transform.Translate(Vector3.forward * this.speed * Time.deltaTime);
    }
}
