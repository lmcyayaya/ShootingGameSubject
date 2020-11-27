﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonFunction : MonoBehaviour
{
    public RectTransform select;
    public Image transitionsTop;
    public Image transitionsDown;
    private Tween selectTween;
    public void OnSelect(Transform pos)
    {
        select.gameObject.SetActive(true);
        selectTween = select.DOMove(pos.position,0.1f);
    }
    public void OnDeselect()
    {
        selectTween.Kill();
    }
    public void ForExit()
    {
        transitionsTop.DOFillAmount(0.5f,0.3f).OnComplete(()=>Application.Quit());
        transitionsDown.DOFillAmount(0.5f,0.3f);
    }
    public void ForRetry()
    {
        transitionsTop.DOFillAmount(0.5f,0.3f).OnComplete(()=>SceneManager.LoadScene(0));
        transitionsDown.DOFillAmount(0.5f,0.3f);
    }
}
