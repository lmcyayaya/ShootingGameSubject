using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Victory : MonoBehaviour
{
    public Image[] buttons;
    public Button retry;
    public Image effect1;
    public Image effect2;
    private void OnEnable()
    {
        effect1.gameObject.SetActive(true);
        effect2.gameObject.SetActive(true);
        
        transform.GetComponent<Image>().DOFade(1,0.3f).SetEase(Ease.OutQuart);
        transform.DOScale(new Vector3(2,2,1),0.3f).SetEase(Ease.OutQuart);

        effect1.DOFade(1,0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f);
        effect1.transform.DOScale(new Vector3(2,2,1),0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f).OnComplete(()=>
        {
            effect1.DOFade(0,0.3f).SetEase(Ease.OutQuart);
            effect1.transform.DOScale(new Vector3(4,4,1),0.3f).SetEase(Ease.OutQuart);
        });
        
        effect2.DOFade(1,0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f);
        effect2.transform.DOScale(new Vector3(2,2,1),0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f).OnComplete(()=>
        {
            effect2.DOFade(0,0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f);
            effect2.transform.DOScale(new Vector3(4,4,1),0.3f).SetEase(Ease.OutQuart).SetDelay(0.1f).OnComplete(()=>
            {
                foreach( Image i in buttons)
                {
                    i.gameObject.SetActive(true);
                    retry.Select();
                    i.DOFade(1,0.3f).SetDelay(0.5f);
                }
            });
        });;
    }

}
