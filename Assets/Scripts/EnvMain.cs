using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvMain : MonoBehaviour
{
    public HeroAgent agent;
    public Target target;
    public SafetyZone safetyZone;

    // Start is called before the first frame update
    void Start()
    {
        this.target.spawnComplete = () =>
        {

        };

        this.safetyZone.onCollide = () =>
        {
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
