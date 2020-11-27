using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartCutIn : MonoBehaviour
{
    public Transform cutIn;
    public Transform cutOut;
    public Transform backGround;
    public Image transitionsTop;
    public Image transitionsDown;
    public bool levelup;
    private void Start()
    {
        transitionsTop.DOFillAmount(0,0.3f);
        transitionsDown.DOFillAmount(0,0.3f).OnComplete(()=>
        {
            transform.DOMove(cutIn.position,0.2f).SetEase(Ease.OutQuart).SetDelay(0.5f).OnComplete(()=>
            {
                backGround.SetParent(transform.parent);
                backGround.DOScale(backGround.localScale*1.5f,0.3f).SetEase(Ease.OutQuart);
                backGround.GetComponent<Image>().DOFade(0,0.3f).SetEase(Ease.OutQuart);
                transform.DOMove(transform.position+Vector3.left*0.5f,1).OnComplete(()=>
                {
                    transform.DOMove(cutOut.position,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
                    {
                        if(levelup)
                            Boss.Instance.scendLevel = true;
                    });
                });
            });
        });
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
