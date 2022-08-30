using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafetyZone : MonoBehaviour
{
    public UnityAction onZombieCollide;
    public UnityAction onCitizenCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zombie") || other.CompareTag("citizen"))
        {
            Destroy(other.gameObject);

            if (other.CompareTag("zombie"))
            {
                this.onZombieCollide();
            }

            if (other.CompareTag("citizen"))
            {
                this.onCitizenCollide();
            }
        }
    }
}
