using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    public static Player Instance
    {
        get {return instance;}
    }
    public State state;
    public float maxHP;
    public float currentHP;
    public float moveSpeed = 0.1f;
    public float dodgeSpeed;
    public float dodgeCD = 0.5f;
    public float shootingRate;
    public LayerMask wallLayer;
    private Vector3 dir;
    private InputHandler inputHandler;
    private Vector3 moveDir;
    private Vector3 lookDir;
    private float timeToFire ;
    private Transform m_transform;
    private float dodgeTimer;
    private Rigidbody2D rb;
    private void Awake() 
    {
        if(instance ==null)
            instance = this;
    }
    void Start()
    {
        m_transform = transform;
        inputHandler = m_transform.GetComponent<InputHandler>();
        rb = m_transform.GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }
    void FixedUpdate()
    {
        switch(state)
        {
            case State.idle:
            {
                rb.velocity = Vector3.zero;
                Shoot();
                Move();
                Rotate();
                Dodge();
                break;
            }
            case State.dodge:
            {
                break;
            }
            case State.damage:
            {
                Move();
                Rotate();
                Dodge();
                break;
            }
        }
    }

    private void Move()
    {
        moveDir =new Vector3(inputHandler.horizontal,inputHandler.vertical,0).normalized;
        rb.velocity = moveDir * moveSpeed ;
    }
    private void Rotate()
    {
        
        dir = transform.up;
        if(inputHandler.lookDir!=Vector3.zero)
            transform.up = inputHandler.lookDir;
        else
            transform.up = dir;
    }
    private void Dodge()
    {
       
        dodgeTimer -=Time.deltaTime;
        if(!inputHandler.dodge || dodgeTimer > 0)
            return; 
        state = State.dodge;
        dodgeTimer = dodgeCD;
        transform.up = moveDir;

        if(!Physics2D.Raycast(m_transform.position,moveDir,dodgeSpeed,wallLayer))
            m_transform.DOMove(m_transform.position+moveDir*dodgeSpeed,0.2f).OnComplete(()=>
            {
                state = State.idle;
            });
        else
        {
            RaycastHit2D hit2D = Physics2D.Raycast(m_transform.position,moveDir,dodgeSpeed,wallLayer);
            m_transform.DOMove(hit2D.point,0.2f).OnComplete(()=>
            {
                state = State.idle;
            });
        }
            
        Vector3 originScale = m_transform.localScale;
        m_transform.localScale = new Vector3(originScale.x*0.2f,originScale.y,originScale.z);
        transform.DOScale(originScale,0.1f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            
        });
  
    }
    private void Shoot()
    {
        if( !inputHandler.fire || Time.time < timeToFire)
            return;
        
        Transform bullet = ObjectPool.TakeFormPool("Bullet");
        bullet.position = m_transform.position+m_transform.up*0.25f;
        bullet.up = transform.up;
        timeToFire = Time.time + shootingRate;

    }
    private void Damage()
    {
        state = State.damage;
        state = State.idle;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag=="EnemyBullet")
            Damage();
    }
    public enum State
    {
        idle,dodge,damage
    }
}
