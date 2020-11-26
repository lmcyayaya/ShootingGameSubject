using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReturnToPool : MonoBehaviour
{
    ParticleSystem ps;
    float t;
    void Start()
    {
        GetParticleSystem();
    }
    private void OnEnable()
    {
        StartCoroutine(ObjectPool.ReturnToPool(this.gameObject,t));
    }
    private void GetParticleSystem()
    {
        if(this.gameObject.GetComponent<ParticleSystem>())
            ps = this.gameObject.GetComponent<ParticleSystem>();
        else
            for(int i = 0;i < this.transform.childCount;i++)
            {
                if(this.transform.GetChild(i).GetComponent<ParticleSystem>())
                {
                    ps = this.transform.GetChild(i).GetComponent<ParticleSystem>();
                    break;
                }
            }
        if(ps!=null)
            t = ps.main.duration;
        else
            Debug.Log(this.gameObject.name+"There aren't Particle System");
    }
}
