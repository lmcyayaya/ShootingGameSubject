using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pixelnest.BulletML;
using System;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    
    protected Transform m_Transform;
    protected SpriteRenderer m_SpriteRenderer;
    private Color originColor;
    protected void Start()
    {
        m_Transform = transform;
        m_SpriteRenderer = m_Transform.GetComponent<SpriteRenderer>();
        originColor = m_SpriteRenderer.color;
        currentHP = maxHP;
    }
    private void Damage(float damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        m_Transform.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Bullet"))
            return;
        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);
        StartCoroutine(HitEffect());
    }
    private IEnumerator HitEffect()
    {
        m_SpriteRenderer.color = new Color(originColor.r,originColor.g,originColor.b,0.7f);
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.color = originColor;
    }
}
