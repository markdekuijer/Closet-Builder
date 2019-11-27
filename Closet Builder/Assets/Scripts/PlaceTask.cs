using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlaceTask : BaseTask
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private string targetTag;
    [SerializeField] private bool SymetricalEntry;
    public Vector3 XYZReverse;
    public Vector3 RotationReverse;

    public GameObject SecretObject;

    public Transform FinalTarget { get { return targetTransform; } }

    public GameObject PreviouslyUsedObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            if (Vector3.Dot(targetTransform.forward, other.transform.forward) > 0.925)
            {
                other.GetComponent<ItemPlacementHelper>()?.ForceInRangeMaterial(true, this);
            }
            else if (SymetricalEntry && Vector3.Dot(targetTransform.forward, other.transform.forward) < -0.925)
            {
                other.GetComponent<ItemPlacementHelper>()?.ForceInRangeMaterial(true, this, true);
            }
            else
            {
                other.GetComponent<ItemPlacementHelper>()?.ForceInRangeMaterial(false, null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            other.GetComponent<ItemPlacementHelper>()?.ForceInRangeMaterial(false, null);
        }
    }
}
