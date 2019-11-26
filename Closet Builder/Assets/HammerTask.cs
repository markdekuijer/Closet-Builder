using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTask : BaseTask
{
    public bool isGroundprotected;
    [SerializeField] private PlaceTask placeTask;
    [SerializeField] private float hitsRequired;

    [SerializeField] private float minTimeBetweenHits;
    [SerializeField] private TargetAxis ScrewAxis;
    [SerializeField] private float depth;

    private float currentTimeBetweenHits;

    private Tween hammerTween;
    private float currentHits;
    private float start;

    protected override void StartTask()
    {
        base.StartTask();
        if (!isGroundprotected)
            start = placeTask.FinalTarget.position.y;
        else
            start = placeTask.FinalTarget.position.x;
    }

    private void DoHit()
    {
        if (currentHits >= hitsRequired)
            return;

        if(hammerTween != null && hammerTween.IsActive())
        {
            hammerTween.Kill();
        }

        currentHits++;
        float t = currentHits / hitsRequired;
        switch (ScrewAxis)
        {
            case TargetAxis.x:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveX(Mathf.Lerp(start, start + depth, t), 0.1f);
                break;
            case TargetAxis.y:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveY(Mathf.Lerp(start, start + depth, t), 0.1f);
                break;
            case TargetAxis.z:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveZ(Mathf.Lerp(start, start + depth, t), 0.1f);
                break;
            case TargetAxis.minx:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveX(Mathf.Lerp(start, start - depth, t), 0.1f);
                break;
            case TargetAxis.miny:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveY(Mathf.Lerp(start, start - depth, t), 0.1f);
                break;
            case TargetAxis.minz:
                hammerTween = placeTask.PreviouslyUsedObject.transform.DOLocalMoveZ(Mathf.Lerp(start, start - depth, t), 0.1f);
                break;
            default:
                break;
        }

        if(currentHits >= hitsRequired)
        {
            hammerTween.OnComplete(() =>
            {
                OnTaskComplete?.Invoke(placeTask.PreviouslyUsedObject);
            });
        }
    }

    private void Update()
    {
        currentTimeBetweenHits += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Hammer"))
        {
            Vector3 dir = collision.gameObject.GetComponent<ItemHammerHelper>().targetTransform.position - transform.position;
            if (currentTimeBetweenHits >= minTimeBetweenHits && Vector3.Dot(dir.normalized, Vector3.up) > 0.9f && isGroundprotected == false)
            {
                DoHit();
            }
            if (currentTimeBetweenHits >= minTimeBetweenHits && Vector3.Dot(dir.normalized, Vector3.right) > 0.9f && isGroundprotected == true)
            {
                DoHit();
            }
        }
    }
}
