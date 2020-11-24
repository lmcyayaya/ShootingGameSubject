using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private static Enemy instance = null;
    public static Enemy Instance
    {
        get {return instance;}
    }
    public float maxHP;
    public float currentHP;
    private Transform m_transform;
    private bool damage;
    private SpriteRenderer m_spritRrenderer;
    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        m_transform = transform;
        m_spritRrenderer = m_transform.GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }
    private void Damage(float damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
            Dead();

    }
    private void Dead()
    {
        transform.gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.tag !="Bullet")
            return;
        
        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);

    }
}
