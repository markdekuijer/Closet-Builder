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
        camFadeimage.DOColor(Color.black, 1f).SetEase(Ease.InQuad).OnComplete(() => { camFadeimage.DOColor(Color.clear, 1f).SetDelay(1f).SetEase(Ease.OutQuad); });
    }
}
