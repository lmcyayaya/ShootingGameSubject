using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHPBar : MonoBehaviour
{
    public Image black;
    public Image red;
    public Color scenedLevelColor;
    private Color originColor;
    private Boss boss;
    private bool hasStart;
    [HideInInspector]public float CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            if(value!=currentHP)
            {
                currentHP = value;
                HPDoFillAmount();
            }
        }
    }
    private float currentHP;

    private void Start()
    {
        transform.DOScale(Vector3.one,1f);
        originColor = red.color;
        boss = Boss.Instance;
    }
    private void Update()
    {
        CurrentHP = boss.currentHP;
        if(Boss.Instance.scendLevel)
        {
            if(!hasStart)
            {
                hasStart= true;
                StartCoroutine(HPEffect());
            }
                
        }
        else
        {
            StopAllCoroutines();
            red.color = originColor;
            hasStart= false;

        }
    }
    private void HPDoFillAmount()
    {
        red.DOFillAmount(boss.currentHP/boss.maxHP,0.4f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            black.DOFillAmount(boss.currentHP/boss.maxHP,0.3f).SetEase(Ease.OutQuart);
        });
    }
    private IEnumerator HPEffect()
    {
        red.DOColor(scenedLevelColor,0.5f);
        yield return new WaitForSeconds(0.5f);
        red.DOColor(originColor,0.5f).OnComplete(()=>
        {
            StartCoroutine(HPEffect());
        });
    }
}
