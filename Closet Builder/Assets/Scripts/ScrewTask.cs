using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewTask : BaseTask
{
    [SerializeField] private float requiredPassedDelta = 15f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private string targetTag;
    [SerializeField] private float targetDepth = 0.028f;
    [SerializeField] private float totalRotation = 1440f;


    [SerializeField] private PlaceTask placeTask;

    public TargetAxis ScrewAxis;

    public PlaceTask PlaceTask => placeTask;
    public float RequiredPassedDelta => requiredPassedDelta;
    public float TotalRotation => totalRotation;
    public Transform TargetTransform => targetTransform;
    public Vector3 FinalTargetPos { get { return targetTransform.position + targetTransform.forward * targetDepth; } }
    public Action OnSuccesfullyPlaced { get; private set; }

    private bool checkCollision = true;
    public GameObject ObjectToScrew
    {
        get
        {
            if(placeTask.PreviouslyUsedObject == null)
            {
                Debug.Log("Didnt find an object that was used");
                return null;
            }

            return placeTask.PreviouslyUsedObject;
        }
    }
    public override void TryStartTask()
    {
        base.TryStartTask();
        OnSuccesfullyPlaced += InitializeRotationState;
    }

    public void InitializeRotationState()
    {
        highlightedIndicator.gameObject.SetActive(false);
        checkCollision = false;
        OnSuccesfullyPlaced -= InitializeRotationState;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!checkCollision)
        {
            return;
        }

        if (other.gameObject.CompareTag(targetTag))
        {
            if (Vector3.Dot(targetTransform.forward, other.transform.forward) > 0.925)
            {
                other.GetComponent<ItemScrewHelper>().ForceInRangeMaterial(true, this);
            }
            else
            {
                other.GetComponent<ItemScrewHelper>().ForceInRangeMaterial(false, null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            other.GetComponent<ItemScrewHelper>()?.ForceInRangeMaterial(false, null);
        }
    }
}

public enum TargetAxis
{
    x, y, z, minx, miny, minz
}