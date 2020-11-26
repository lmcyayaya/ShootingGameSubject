using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHPBar : MonoBehaviour
{
    public Image black;
    public Image red;
    private Boss boss;
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
        boss = Boss.Instance;
    }
    private void Update()
    {
        CurrentHP = boss.currentHP;
    }
    private void HPDoFillAmount()
    {
        red.DOFillAmount(boss.currentHP/boss.maxHP,0.4f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            black.DOFillAmount(boss.currentHP/boss.maxHP,0.3f).SetEase(Ease.OutQuart);
        });
    }
}
