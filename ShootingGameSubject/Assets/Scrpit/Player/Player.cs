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
    public bool invincible;
    public float invincibleTime;
    public LayerMask wallLayer;
    public ParticleSystem deadParticle;
    public GameObject gameOver;
    public GameObject canvas;
    private InputHandler inputHandler;
    private Vector3 dir;
    private Vector3 moveDir;
    private Vector3 lookDir;
    private float timeToFire;
    private float dodgeTimer;
    private float invincibleTimer;
    private Rigidbody2D m_Rb;
    private SpriteRenderer m_SpriteRenderer;
    private Collider2D m_Collider;
    [HideInInspector]public Transform m_Transform;
    private void Awake() 
    {
        if(instance ==null)
            instance = this;
    }
    private void Start()
    {
        m_Transform = transform;
        inputHandler = m_Transform.GetComponent<InputHandler>();
        m_Rb = m_Transform.GetComponent<Rigidbody2D>();
        m_SpriteRenderer = m_Transform.GetComponent<SpriteRenderer>();
        m_Collider = m_Transform.GetComponent<Collider2D>();
        currentHP = maxHP;
    }
    private void FixedUpdate()
    {
        switch(state)
        {
            case State.idle:
            {
                m_Rb.velocity = Vector3.zero;
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
            case State.dead:
            {
                break;
            }
        }
        invincibleTimer -=Time.deltaTime;
        if(invincibleTimer <=0)
        {
            m_Collider.isTrigger = false;
            invincible = false;
        }
            

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Dead();
        }
    }

    private void Move()
    {
        moveDir =new Vector3(inputHandler.horizontal,inputHandler.vertical,0).normalized;
        m_Rb.velocity = moveDir * moveSpeed ;
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

        if(!Physics2D.Raycast(m_Transform.position,moveDir,dodgeSpeed,wallLayer))
            m_Transform.DOMove(m_Transform.position+moveDir*dodgeSpeed,0.2f).OnComplete(()=>
            {
                state = State.idle;
            });
        else
        {
            RaycastHit2D hit2D = Physics2D.Raycast(m_Transform.position,moveDir,dodgeSpeed,wallLayer);
            m_Transform.DOMove(hit2D.point,0.2f).OnComplete(()=>
            {
                state = State.idle;
            });
        }
            
        Vector3 originScale = m_Transform.localScale;
        m_Transform.localScale = new Vector3(originScale.x*0.2f,originScale.y,originScale.z);
        transform.DOScale(originScale,0.2f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            
        });
  
    }
    private void Shoot()
    {
        if( !inputHandler.fire || Time.time < timeToFire)
            return;
        
        Transform bullet = ObjectPool.TakeFromPool("Bullet");
        bullet.position = m_Transform.position+m_Transform.up*0.25f;
        bullet.up = transform.up;
        timeToFire = Time.time + shootingRate;

    }
    private void Dead()
    {

        invincible = true;
        invincibleTimer = invincibleTime;
        m_Collider.isTrigger = true;
        state = State.dead;
        m_Rb.velocity = Vector3.zero;
        canvas.SetActive(false);

        StartCoroutine(DamageEffect());

        Time.timeScale = 0.2f;
        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.3f,0.6f,0.15f));
        Camera.main.transform.parent.DOMove(new Vector3(m_Transform.position.x,m_Transform.position.y,-10),0.6f).OnComplete(()=>
        {
            Time.timeScale = 1;
            deadParticle.transform.SetParent(null);
            deadParticle.gameObject.SetActive(true);
            deadParticle.Play();
            gameOver.SetActive(true);
            m_Transform.gameObject.SetActive(false);
            
        });
        Camera.main.DOOrthoSize(1.5f,0.6f);
        
    }
    private void Damage(Vector3 damagePos,float damage)
    {
        if(state == State.dodge || state==State.damage || invincible)
            return;

        if(currentHP-damage<=0)
        {
            Dead();
            return;
        }

        invincible = true;
        invincibleTimer = invincibleTime;
        m_Collider.isTrigger = true;

        state = State.damage;
        StartCoroutine(DamageEffect());
        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.4f,0.05f,1.5f));
        currentHP -= damage;
        Vector3 damageDir = (transform.position - damagePos).normalized;
        if(!Physics2D.Raycast(m_Transform.position,damageDir,1f,wallLayer))
        {
            m_Transform.DOMove(m_Transform.position+damageDir,0.05f).OnComplete(()=>
            {
                state = State.idle;
            });
        }
        else
        {
            RaycastHit2D hit2D = Physics2D.Raycast(m_Transform.position,damageDir,1,wallLayer);
            m_Transform.DOMove(hit2D.point,0.05f).OnComplete(()=>
            {
                state = State.idle;
            });
        }
    }
    IEnumerator DamageEffect()
    {
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.color =new Color(1,1,1,0);
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.color =new Color(1,1,1,1);
        if(invincible)
            StartCoroutine(DamageEffect());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("EnemyBullet")||other.gameObject.CompareTag("Enemy"))
            Damage(other.transform.position,other.transform.GetComponent<EnemyBullet>().damage);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("EnemyBullet")||other.gameObject.CompareTag("Enemy"))
            Damage(other.transform.position,other.transform.GetComponent<EnemyBullet>().damage);
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("EnemyBullet")||other.gameObject.CompareTag("Enemy"))
            Damage(other.transform.position,other.transform.GetComponent<EnemyBullet>().damage);
    }
    public enum State
    {
        idle,dodge,damage,dead
    }
}
