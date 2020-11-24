using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHPBar : MonoBehaviour
{
    public Image black;
    public Image red;
    private Enemy enemy;
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
    void Start()
    {
        enemy = Enemy.Instance;
    }
    void Update()
    {
        CurrentHP = enemy.currentHP;
    }
    private void HPDoFillAmount()
    {
        red.DOFillAmount(enemy.currentHP/enemy.maxHP,0.4f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            black.DOFillAmount(enemy.currentHP/enemy.maxHP,0.3f).SetEase(Ease.OutQuart);
        });
    }
}
