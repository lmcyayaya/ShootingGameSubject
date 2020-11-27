using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameOver : MonoBehaviour
{
    public Image[] buttons;
    public Button retry;
    private Image m_Image;
    private Vector3 originScale;
    private void OnEnable() 
    {
        m_Image.DOFade(1,0.2f).SetEase(Ease.OutQuart).SetDelay(0.1f);
        m_Image.rectTransform.DOScale(new Vector3(2,2,1),0.2f).SetDelay(0.1f).OnComplete(()=>
        {
            
            foreach( Image i in buttons)
            {
                i.gameObject.SetActive(true);
                i.DOFade(1,0.3f).SetDelay(0.5f).OnComplete(()=>retry.Select());
            }
        });
        
    }
    private void OnDisable() 
    {
        m_Image.DOFade(0,0);
        m_Image.rectTransform.localScale = originScale;
    }
    private void Awake()
    {
        m_Image = transform.GetComponent<Image>();
        originScale = m_Image.rectTransform.localScale;
    }
    
}
