using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    private Transform m_transform;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D rb;
    void Start()
    {
        m_transform = transform;
        m_spriteRenderer = m_transform.GetComponent<SpriteRenderer>();
        rb = m_transform.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.velocity = m_transform.up * bulletSpeed;
        if(!m_spriteRenderer.isVisible)
            ObjectPool.ReturnToPool(m_transform.gameObject);
    }
}
