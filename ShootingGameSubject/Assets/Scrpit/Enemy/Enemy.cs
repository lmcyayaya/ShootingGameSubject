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
    public LayerMask wallLayer;
    public Color hitColor;
    protected bool acting;
    protected Transform m_Transform;
    protected SpriteRenderer m_SpriteRenderer;
    private Color originColor;
    protected void Awake()
    {
        m_Transform = transform;
        m_SpriteRenderer = m_Transform.GetComponent<SpriteRenderer>();
        originColor = m_SpriteRenderer.color;
        currentHP = maxHP;
    }
    private void OnEnable() 
    {
        acting = true;
        m_SpriteRenderer.color = originColor;
        currentHP = maxHP;
        StartCoroutine(WaitToDo(0.8f,()=> acting = false));
    }
    private void OnDisable() 
    {
        StopAllCoroutines();
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
        currentHP = maxHP;
        Boss.Instance.destoryEnemy +=1;
        Transform deadParticle = ObjectPool.TakeFromPool("DeadParticle");
        deadParticle.position = m_Transform.position;
        
        ObjectPool.ReturnToPool(m_Transform.gameObject);
        //m_Transform.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Bullet"))
            return;
            
        StartCoroutine(HitEffect());
        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);
        
    }
    private IEnumerator HitEffect()
    {
        m_SpriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.color = originColor;
    }
    protected IEnumerator WaitToDo(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
