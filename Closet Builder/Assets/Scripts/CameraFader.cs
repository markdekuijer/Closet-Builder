using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraFader : Singleton<CameraFader>
{
    [SerializeField] private Image camFadeimage;

    public void SwitchSequence()
    {
        camFadeimage.gameObject.SetActive(true);
        camFadeimage.DOFade(1, 2f).SetEase(Ease.InQuad).OnComplete(() => { camFadeimage.DOFade(0, 2f).SetDelay(2f).SetEase(Ease.OutQuad).OnComplete(() => { camFadeimage.gameObject.SetActive(false); }); });
    }
}
