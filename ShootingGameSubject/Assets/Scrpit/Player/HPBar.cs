using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    public Image red;
    public Image green;
    private Player player;
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
        player = Player.Instance;
    }
    private void Update()
    {
        CurrentHP = player.currentHP;
    }
    private void HPDoFillAmount()
    {
        green.DOFillAmount(player.currentHP/player.maxHP,0.4f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            red.DOFillAmount(player.currentHP/player.maxHP,0.3f).SetEase(Ease.OutQuart);
        });
    }
}
