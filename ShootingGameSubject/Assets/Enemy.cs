using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform m_transform;
    void Start()
    {
        m_transform = transform;
    }
    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.tag !="Bullet")
            return;

        m_transform.gameObject.SetActive(false);
        ObjectPool.ReturnToPool(other.gameObject);

    }
}
