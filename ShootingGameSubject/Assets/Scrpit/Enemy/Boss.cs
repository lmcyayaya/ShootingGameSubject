using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pixelnest.BulletML;
using System;

public class Boss : MonoBehaviour
{
    private static Boss instance = null;
    public static Boss Instance
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
    public Transform[] openingPoint;
    public Transform shield;
    public Color hitColor;
    public int destoryEnemy;
    private Transform m_Transform;
    private bool checkEnemy;
    private SpriteRenderer m_SpriteRenderer;
    private Color originColor;
    private Vector3 originScale;
    private bool tsunamiEnd;
    private List<Action> attackList = new List<Action>();
    private Action lastAttack;
    private Action attackLaser;
    private Action attackTsunami;
    private Action attackCallEnemy;
    private int attackTimes;
    private void Awake() 
    {
        instance = this;
    }
    private void Start()
    {
        m_Transform = transform;
        m_SpriteRenderer = m_Transform.GetComponent<SpriteRenderer>();
        originScale = m_Transform.localScale;
        originColor = m_SpriteRenderer.color;
        currentHP = maxHP;
        attackLaser = Laser;
        attackTsunami = Tsunami;
        attackCallEnemy = CallEnemy;
        attackList.Add(attackLaser);
        attackList.Add(attackTsunami);
        attackList.Add(attackCallEnemy);
        m_Transform.localScale = new Vector3(0,0,1);
        StartCoroutine(WaitToDo(3,()=>
        {
            state = State.idle; 
        }));
        
    }
    private void FixedUpdate() 
    {
        switch(state)
        {
            case State.idle:
            {
                
                SelectAttack();
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
        if(!scendLevel)
        {
            if(checkEnemy)
            {
                if(destoryEnemy >= 3)
                {
                    destoryEnemy = 0;
                    m_Transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).OnComplete(()=>
                    {
                        shield.gameObject.SetActive(false);
                        state = State.idle;
                    });
                }
            }
            else
            {
                if(destoryEnemy >= 2)
                {
                    destoryEnemy = 0;
                    m_Transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).OnComplete(()=>
                    {
                        shield.gameObject.SetActive(false);
                        state = State.idle;
                    });
                }
            }
        }  
        else
        {
            if(checkEnemy)
                if(destoryEnemy >= 6)
                {
                    destoryEnemy = 0;
                    m_Transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).OnComplete(()=>
                    {
                        shield.gameObject.SetActive(false);
                        state = State.idle;
                    });
                }
            else
            {
                if(destoryEnemy >= 3)
                {
                    destoryEnemy = 0;
                    m_Transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).OnComplete(()=>
                    {
                        destoryEnemy = 0;
                        shield.gameObject.SetActive(false);
                        state = State.idle;
                    });
                }
            }  
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
        m_Transform.gameObject.SetActive(false);
    }
    private void SelectAttack()
    {
        attackTimes+=1;
        int attackIndex = UnityEngine.Random.Range(0,attackList.Count);
        while (attackList[attackIndex]==lastAttack && attackList[attackIndex]!=Laser|| attackTimes < 5 && attackList[attackIndex]==CallEnemy)
        {
            attackIndex = UnityEngine.Random.Range(0,attackList.Count);
        }
        attackList[attackIndex]();
    }
    private void Laser()
    {
        lastAttack = Laser;
        state = State.attacking;
        int dir =UnityEngine.Random.Range(0,4);
        m_Transform.localScale = originScale;

        switch(dir)
        {
            case 0:
            {
                m_Transform.position = laserPoint[0].position;
                m_Transform.rotation = laserPoint[0].rotation;
                m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_Transform.DOMove(m_Transform.position+Vector3.left*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_Transform.DOMove(m_Transform.position+Vector3.right*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
                m_Transform.position = laserPoint[1].position;
                m_Transform.rotation = laserPoint[1].rotation;
                m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_Transform.DOMove(m_Transform.position+Vector3.right*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_Transform.DOMove(m_Transform.position+Vector3.left*16.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
                m_Transform.position = laserPoint[2].position;
                m_Transform.rotation = laserPoint[2].rotation;

                m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_Transform.DOMove(m_Transform.position+Vector3.down*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_Transform.DOMove(m_Transform.position+Vector3.up*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
                m_Transform.position = laserPoint[3].position;
                m_Transform.rotation = laserPoint[3].rotation;

                m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            m_Transform.DOMove(m_Transform.position+Vector3.up*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                if(!scendLevel)
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                    });
                                else
                                    m_Transform.DOMove(m_Transform.position+Vector3.down*8.5f,0.5f).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
        lastAttack = Tsunami;
        tsunamiEnd = false;
        state = State.attacking;
        m_Transform.position = new Vector3(0,1,0);
        m_Transform.rotation = Quaternion.Euler(0,0,0);
        m_Transform.localScale = new Vector3(0,0,1);
        
        m_Transform.DOScale(originScale,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            m_Transform.GetComponent<BulletSourceScript>().Initialize();
            Coroutine tsunamiEffect =  StartCoroutine(TsunamiEffect());
            m_Transform.DOScale(new Vector3(0,0,1),0.3f).SetEase(Ease.InBack).SetDelay(5).OnComplete(()=>
            {
                state = State.idle;
                tsunamiEnd = true;
                StopCoroutine(tsunamiEffect);
            });
        });
    }
    private void CallEnemy()
    {
        
        attackTimes = 0;
        lastAttack = CallEnemy;
        state = State.attacking;

        m_Transform.position = new Vector3(0,1,0);
        m_Transform.rotation = Quaternion.Euler(0,0,0);
        m_Transform.localScale = new Vector3(0,0,1);
        
        shield.gameObject.SetActive(true);

        m_Transform.DOScale(originScale,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            shield.SetParent(null);

            int enemyIndex = UnityEngine.Random.Range(0,2);
            switch(enemyIndex)
            {
                case 0:
                    checkEnemy = true;
                    if(scendLevel)
                        StartCoroutine(SpawnEnemy("NormalEnemy",6,-m_Transform.up));
                    else
                        StartCoroutine(SpawnEnemy("NormalEnemy",3,-m_Transform.up));
                    break;
                case 1:
                    checkEnemy = false;
                    if(scendLevel)
                        StartCoroutine(SpawnEnemy("DashEnemy",3,-m_Transform.up));
                    else
                        StartCoroutine(SpawnEnemy("DashEnemy",2,-m_Transform.up));
                    break;

            }
        });
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Bullet"))
            return;

        Damage(other.transform.GetComponent<Bullet>().damage);
        ObjectPool.ReturnToPool(other.gameObject);
        StartCoroutine(HitEffect());
    }
    public enum State
    {
        idle,attacking,dead
    }
    private IEnumerator SpawnEnemy(string target,float quantity,Vector3 dir)
    {
        m_Transform.DOScale(originScale*0.8f,0.15f).OnComplete(()=>
        {
            m_Transform.DOScale(originScale,0.15f).SetEase(Ease.OutBack);
        });
        Transform targetEnemy = ObjectPool.TakeFromPool(target);
        targetEnemy.position = m_Transform.position;
        targetEnemy.up = dir;
        targetEnemy.DOMove(m_Transform.position+dir*3f,0.8f).SetEase(Ease.OutQuart);

        yield return new WaitForSeconds(1);
        if(quantity-1>0)
            StartCoroutine(SpawnEnemy(target,quantity-1,Quaternion.Euler(0,0,-120)*dir));
        else
            shield.SetParent(m_Transform);
            
            

    }
    private IEnumerator TsunamiEffect()
    {
        if(tsunamiEnd)
        {
            yield break;
        }
            
        m_Transform.Rotate(Vector3.forward*18);
        yield return new WaitForSeconds(Time.deltaTime);
            StartCoroutine(TsunamiEffect());
    }
    private IEnumerator HitEffect()
    {
        m_SpriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.color = originColor;
    }
    private IEnumerator WaitToDo(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
