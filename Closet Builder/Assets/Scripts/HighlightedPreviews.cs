using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedPreviews : MonoBehaviour
{
    [Header("Inserting")]
    [SerializeField] private float insertDepth;
    [SerializeField] private float insertionTime;

    [Header("Rotating")]
    [SerializeField] private bool waitForInsertion;
    [SerializeField] private float totalRotationAngle;
    [SerializeField] private float rotationTime;

    [Header("Overall Delays")]
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;

    [SerializeField] private MovementAxis axis;

    void Start()
    {
        if(insertDepth == 0 && totalRotationAngle == 0)
        {
            return;
        }

        Sequence screwSequence = DOTween.Sequence();
        screwSequence.PrependInterval(startDelay);
        switch (axis)
        {
            case MovementAxis.X:
                screwSequence.Append(transform.DOLocalMoveX(transform.localPosition.x - insertDepth, insertionTime).SetEase(Ease.InSine));
                break;
            case MovementAxis.Y:
                screwSequence.Append(transform.DOLocalMoveY(transform.localPosition.y - insertDepth, insertionTime).SetEase(Ease.InSine));
                break;
            case MovementAxis.Z:
                screwSequence.Append(transform.DOLocalMoveZ(transform.localPosition.z - insertDepth, insertionTime).SetEase(Ease.InSine));
                break;
        }

        if(waitForInsertion)
            screwSequence.Append(transform.DORotate(new Vector3(0,totalRotationAngle, 0), rotationTime, RotateMode.LocalAxisAdd));
        else
            screwSequence.Insert(0, transform.DORotate(new Vector3(0, totalRotationAngle, 0), rotationTime, RotateMode.LocalAxisAdd));

        screwSequence.AppendInterval(endDelay);
        screwSequence.SetLoops(-1, LoopType.Restart);
    }

    public void DestroyTweener()
    {
        Destroy(gameObject);
    }
}

public enum MovementAxis
{
    X, Y, Z
}
