using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvMain : MonoBehaviour
{
    public HeroAgent agent;
    public Target target;
    public SafetyZone safetyZone;
    public GameObject groundGo;
    public TextMeshPro cumulativeRewardText;

    private Renderer groundRenderer;
    public Material redMat;
    public Material greenMat;
    public Material groundMat;


    void Start()
    {
        this.groundRenderer = this.groundGo.GetComponent<Renderer>();

        //this.target.spawnComplete = () =>
        //{
        //    this.agent.SetReward(10f);
        //    this.agent.EndEpisode();
        //    this.target.CreateTarget();
        //};

        this.safetyZone.onZombieCollide = () =>
        {
            StartCoroutine(this.ChangeGroundColor(this.redMat));
            this.agent.AddReward(-1);
            this.agent.EndEpisode();
        };

        this.safetyZone.onCitizenCollide = () =>
        {
            StartCoroutine(this.ChangeGroundColor(this.greenMat));
            this.agent.AddReward(1);
        };
    }

    void Update()
    {
        this.cumulativeRewardText.text = this.agent.GetCumulativeReward().ToString("0.000");
    }

    private IEnumerator ChangeGroundColor(Material mat)
    {
        this.groundRenderer.material = mat;
        yield return new WaitForSeconds(0.3f);
        this.groundRenderer.GetComponent<Renderer>().material = this.groundMat;
    }
}
