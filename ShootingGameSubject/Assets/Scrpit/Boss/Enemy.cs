using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pixelnest.BulletML;
using System;

public class Enemy : MonoBehaviour
{
    private static Enemy instance = null;
    public static Enemy Instance
    {
        get {return instance;}
    }
    public State state;
    public float maxHP;
    public float currentHP;
    public bool scendLevel;
    public BulletManagerScript bulletMLManager;
    public Transform laser;
    public Transform[] laserPoint;
    private Transform m_transform;
    private SpriteRenderer m_spritRrenderer;
    private Vector3 originScale;
    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        m_transform = transform;
        m_spritRrenderer = m_transform.GetComponent<SpriteRenderer>();
        originScale = m_transform.localScale;
        currentHP = maxHP;
        StartCoroutine(WaitToDo(1,()=>state = State.idle));
    }
    private void FixedUpdate() 
    {
        switch(state)
        {
            case State.idle:
            {
                int randomAttack = UnityEngine.Random.Range(0,2);
                switch(randomAttack)
                {
                    case 0:
                        Laser();
                        break;
                    case 1:
                        Tsunami();
                        break;
                    case 2:
                        break;
                }
                
                break;
            }
            case State.attacking:
            {
                break;
            }
            case State.dead:
            {
                break;
            }
        }
        if(currentHP < maxHP/2)
            scendLevel = true;
        if(scendLevel)
        {
            bulletMLManager.timeSpeed = 2;
        }
     
    }
    private void Damage(float damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            state = State.dead;
            Dead();
        }
    }
    private void Dead()
    {
        transform.gameObject.SetActive(false);
    }
    private void Laser()
    {
        state = State.attacking;
        int dir =UnityEngine.Random.Range(0,4);
        m_transform.localScale = originScale;
        switch(dir)
        {
            case 0:
            {
                m_transform.position = laserPoint[0].position;
                m_transform.rotation = laserPoint[0].rotation;
                m_transform.DOMove(m_transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_transform.DOMove(m_transform.position+Vector3.left*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_transform.DOMove(m_transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_transform.DOMove(m_transform.position+Vector3.right*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_transform.DOMove(m_transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                
                            });
                        });
                    })));
                });
                break;
            }
            case 1:
            {
                m_transform.position = laserPoint[1].position;
                m_transform.rotation = laserPoint[1].rotation;
                m_transform.DOMove(m_transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_transform.DOMove(m_transform.position+Vector3.right*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_transform.DOMove(m_transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_transform.DOMove(m_transform.position+Vector3.left*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_transform.DOMove(m_transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                
                            });
                        });
                    })));
                });
                
                break;
            }
            case 2:
            {
                m_transform.position = laserPoint[2].position;
                m_transform.rotation = laserPoint[2].rotation;

                m_transform.DOMove(m_transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_transform.DOMove(m_transform.position+Vector3.down*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_transform.DOMove(m_transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_transform.DOMove(m_transform.position+Vector3.up*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_transform.DOMove(m_transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                
                            });
                        });
                    })));
                });
                break;
            }
            case 3:
            {
                m_transform.position = laserPoint[3].position;
                m_transform.rotation = laserPoint[3].rotation;

                m_transform.DOMove(m_transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_transform.DOMove(m_transform.position+Vector3.up*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_transform.DOMove(m_transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_transform.DOMove(m_transform.position+Vector3.down*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_transform.DOMove(m_transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                
                            });
                        });
                    })));
                }); 
                break;
            }

        }
    }
    private void Tsunami()
    {
        state = State.attacking;
        m_transform.position = new Vector3(0,1,0);
        m_transform.rotation = Quaternion.Euler(0,0,0);
        m_transform.localScale = new Vector3(0,0,1);
        
        m_transform.DOScale(originScale,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            m_transform.GetComponent<BulletSourceScript>().Initialize();
            StartCoroutine(TsunamiEffect());
            m_transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).SetDelay(4).OnComplete(()=>
            {
                state = State.idle;
                StopCoroutine(TsunamiEffect());
            });
        });
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag !="Bullet")
            return;
        
        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);
    }
    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.tag !="Bullet")
            return;
        
        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);

    }
    public enum State
    {
        idle,attacking,dead
    }
    IEnumerator TsunamiEffect()
    {
        m_transform.Rotate(Vector3.forward*18);
        yield return new WaitForSeconds(0.02f);
        if(state == State.attacking)
            StartCoroutine(TsunamiEffect());
    }
    IEnumerator WaitToDo(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
