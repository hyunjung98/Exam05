using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafetyZone : MonoBehaviour
{
    public UnityAction onCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zombie") || other.CompareTag("citizen"))
        {
            Destroy(other.gameObject);
            this.onCollide();
        }
    }
}
