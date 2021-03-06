﻿using System.Collections;
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
    [Header("Boss State")]
    public State state;
    public float maxHP;
    public float currentHP;
    public float scendLevelLine;
    public bool scendLevel;
    [Header("Boss Attack Value")]
    public float laserTime = 0.7f;
    public float waveTime;
    public int normalEnemy;
    public int dashEnemy;
    public int scendLevelNormalEnemy;
    public int scendLevelDashEnemy;
    [Header("")]
    public BulletManagerScript bulletMLManager;
    public ParticleSystem deadParticle;
    public GameObject canvas;
    public GameObject victory;
    public GameObject levelup;
    public Transform laser;
    public Transform[] laserPoint;
    public Transform[] rightWaves;
    public Transform[] leftWaves;
    public Transform shield;
    public Color hitColor;
    public int destoryEnemy;
    private Transform m_Transform;
    private SpriteRenderer m_SpriteRenderer;
    private Color originColor;
    private Vector3 originScale;
    private bool checkEnemy;
    private bool tsunamiEnd;
    private bool waveEnd;
    private List<Action> attackList = new List<Action>();
    private Action lastAttack;
    private int attackTimes;
    private Tween currentTween;
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
        attackList.Add(Laser);
        attackList.Add(Laser);
        attackList.Add(Tsunami);
        attackList.Add(CallEnemy);
        m_Transform.localScale = new Vector3(0,0,1);
        StartCoroutine(WaitToDo(4,()=>
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

                ChangeLvevl();
                if(Player.Instance.state!=Player.State.dead)
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
        
        if(!scendLevel)
        {
            if(checkEnemy)
            {
                if(destoryEnemy >= normalEnemy)
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
                if(destoryEnemy >= dashEnemy)
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
            {
                if(destoryEnemy >= scendLevelNormalEnemy)
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
                if(destoryEnemy >= scendLevelDashEnemy)
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
    }
    private void ChangeLvevl()
    {
        if(currentHP < maxHP*scendLevelLine/100 && scendLevel)
        {
            bulletMLManager.timeSpeed = 2;
            if(!attackList.Contains(Wave))
            {
                attackList.Add(Wave);
                attackList.Add(Wave);
            }
        }
        else if(currentHP < maxHP*scendLevelLine/100 && !scendLevel)
        {
            state = State.attacking;
            scendLevel = true;
        }
        
    }

    private void SelectAttack()
    {
        attackTimes+=1;
        int attackIndex = UnityEngine.Random.Range(0,attackList.Count);
        while (attackList[attackIndex]==lastAttack && attackList[attackIndex]!=Laser|| attackTimes < 3 && attackList[attackIndex]==CallEnemy)
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
                currentTween = m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.left*16.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                if(scendLevel)
                                    currentTween = m_Transform.DOMove(m_Transform.position+Vector3.right*16.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                else
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        currentTween = m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
                currentTween = m_Transform.DOMove(m_Transform.position+Vector3.up*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,10,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.right*16.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                if(scendLevel)
                                    currentTween = m_Transform.DOMove(m_Transform.position+Vector3.left*16.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                        laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                else  
                                    laser.DOScale(new Vector3(0,10,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        currentTween = m_Transform.DOMove(m_Transform.position+Vector3.down*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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

                currentTween = m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.2f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.down*8.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                if(scendLevel)
                                   currentTween = m_Transform.DOMove(m_Transform.position+Vector3.up*8.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                else
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        currentTween = m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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

                currentTween = m_Transform.DOMove(m_Transform.position+Vector3.left*1.2f,0.5f).SetEase(Ease.OutBack).OnComplete(()=>
                {
                    laser.DOScale(new Vector3(0.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1f,1f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>laser.DOScale(new Vector3(1.5f,0.5f,1),0.3f).SetEase(Ease.OutBack).OnComplete(()=>
                    {
                        laser.DOScale(new Vector3(1.5f,36,1),0.3f).SetEase(Ease.InExpo).OnComplete(()=>
                        {
                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.up*8.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                            {
                                StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                if(scendLevel)
                                    currentTween = m_Transform.DOMove(m_Transform.position+Vector3.down*8.5f,laserTime).SetEase(Ease.InQuart).OnComplete(()=>
                                    {
                                        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.03f,0.15f));
                                        laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                        {
                                            laser.localScale = new Vector3(0,0,1);
                                            currentTween = m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
                                        });
                                    });
                                else
                                    laser.DOScale(new Vector3(0,36,1),1f).SetEase(Ease.OutQuart).OnComplete(()=>
                                    {
                                        laser.localScale = new Vector3(0,0,1);
                                        currentTween = m_Transform.DOMove(m_Transform.position+Vector3.right*1.2f,0.5f).SetEase(Ease.InBack).OnComplete(()=> state = State.idle);
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
                        StartCoroutine(SpawnEnemy("NormalEnemy",scendLevelNormalEnemy,-m_Transform.up));
                    else
                        StartCoroutine(SpawnEnemy("NormalEnemy",normalEnemy,-m_Transform.up));
                    break;
                case 1:
                    checkEnemy = false;
                    if(scendLevel)
                        StartCoroutine(SpawnEnemy("DashEnemy",scendLevelDashEnemy,-m_Transform.up));
                    else
                        StartCoroutine(SpawnEnemy("DashEnemy",dashEnemy,-m_Transform.up));
                    break;

            }
        });
        
    }
    private void Wave()
    {
        state = State.attacking;
        lastAttack = Wave;
        m_Transform.position = new Vector3(0,1,0);
        m_Transform.rotation = Quaternion.Euler(0,0,0);
        m_Transform.localScale = new Vector3(0,0,1);
        m_Transform.DOScale(originScale,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            currentTween = m_Transform.DOMove(new Vector3(0,4.5f,0),waveTime).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                currentTween = m_Transform.DOMove(new Vector3(0,-3.6f,0),0.3f).SetEase(Ease.InQuart).OnComplete(()=>
                {
                    StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.5f,0.05f,0.15f));
                    StartCoroutine(WaveAttack(rightWaves,0,()=>
                    {
                        currentTween = m_Transform.DOMove(m_Transform.position-Vector3.up*1.2f,0.3f).SetEase(Ease.InBack).OnComplete(()=>
                        {
                            state = State.idle;
                        });
                    }));
                    StartCoroutine(WaveAttack(leftWaves,0,null));

                });
            });
        });
    }
    private void Damage(float damage)
    {
        if(currentHP < maxHP*scendLevelLine/100 && !scendLevel)
        {
            levelup.SetActive(true);
            return;
        }
            

        currentHP -= damage;
        if(currentHP <= 0)
        {
            state = State.dead;
            Player.Instance.state = Player.State.dead;
            Dead();
        }
    }
    private void Dead()
    {
        StopAllCoroutines();
        currentTween.Kill();
        canvas.SetActive(false);
        
        Time.timeScale = 0.2f;
        StartCoroutine(CameraShaker.Instance.CameraShakeOneShot(0.3f,0.55f,0.15f));
        Camera.main.transform.parent.DOMove(new Vector3(m_Transform.position.x,m_Transform.position.y,-10),0.6f).OnComplete(()=>
        {
            Time.timeScale = 1;
            deadParticle.gameObject.SetActive(true);
            deadParticle.transform.SetParent(null);
            deadParticle.Play();
            
            victory.SetActive(true);

            m_Transform.gameObject.SetActive(false);
            
        });
        Camera.main.DOOrthoSize(1.5f,0.6f);
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
    private IEnumerator WaveAttack(Transform[] waves,int index,Action action)
    {
        waves[index].DOScale(new Vector3(0.5f,4.5f,1),0.3f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            waves[index].DOScale(new Vector3(0.5f,0,1),0.3f).SetEase(Ease.InQuart).OnComplete(()=>
            {
                if(index==waves.Length-1)
                {
                    if(action!=null)
                        action();
                }
            });
        });
        yield return new WaitForSeconds(0.1f);
            if(index+1 < waves.Length)
                StartCoroutine(WaveAttack(waves,index+1,action));
            else
                yield break;
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
