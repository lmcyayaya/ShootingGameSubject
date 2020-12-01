using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Title : MonoBehaviour
{
    private static Title instance = null;
    public static Title Instance
    {
        get {return instance;}
    }
    public Image title;
    public Image kadai;
    public Image two;
    public Image hard;
    public Image normal;
    public Image diffcultSelect;
    public GameObject[] hasActive;
    public RectTransform particle;
    private Tween check;
    void Start()
    {
        instance = this;
        particle.gameObject.SetActive(true);
        title.DOFillAmount(1,.5f).SetEase(Ease.OutQuad).OnComplete(()=>
        {
            two.DOFillAmount(1,0.3f).SetEase(Ease.OutQuart).SetDelay(0.3f);
            kadai.DOFade(1,0.3f).SetEase(Ease.OutQuart);
            kadai.rectTransform.DOScale(new Vector3(0.5f,0.5f,1),0.4f).SetEase(Ease.OutBack).OnComplete(()=>
            {
                hard.DOFade(1,0.5f).SetEase(Ease.OutQuad);
                normal.DOFade(1,0.5f).SetEase(Ease.OutQuad);
                diffcultSelect.DOFade(1,0.5f).SetEase(Ease.OutQuad);
            });
            
        }); 
    }
    void Update()
    {
        if(title.fillAmount!=1)
            particle.anchoredPosition = new Vector3(-310+title.fillAmount*620,0,0);
        else
            particle.gameObject.SetActive(false);
    }
    public void AllFade(Action action)
    {
        if(check!=null)
            return;
        check = title.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        kadai.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        two.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        normal.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        hard.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        diffcultSelect.DOFade(0,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            normal.gameObject.SetActive(false);   
            hard.gameObject.SetActive(false);
            diffcultSelect.gameObject.SetActive(false);
            ActiveGameObject();
            action();
        });
    }
        private void ActiveGameObject()
    {
        foreach(GameObject obj in hasActive)
        {
            obj.SetActive(true);
        }
    }
}
