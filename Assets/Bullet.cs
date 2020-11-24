using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    private Transform m_transform;
    private SpriteRenderer m_spriteRenderer;
    void Start()
    {
        m_transform = transform;
        m_spriteRenderer = m_transform.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        m_transform.position += m_transform.up * bulletSpeed * Time.deltaTime;
        if(!m_spriteRenderer.isVisible)
            ObjectPool.ReturnToPool(m_transform.gameObject);
    }
}
