using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ItemPlacementHelper : MonoBehaviour
{
    [SerializeField] private Material inrangeMaterial;

    private Material standartMaterial;
    private Renderer materialRenderer;
    private bool inRange;
    private bool completed;
    private PlaceTask task;

    private void Start()
    {
        materialRenderer = GetComponentInChildren<Renderer>();
    }

    public void ForceInRangeMaterial(bool shouldForce, PlaceTask placeTask)
    {
        inRange = shouldForce;

        if (inRange)
        {
            task = placeTask;
        }
        else
        {
            task = null;
            materialRenderer.material = standartMaterial;
        }
    }

    public void OnGrab()
    {
        if(standartMaterial == null)
        {
            standartMaterial = materialRenderer.material;
        }
    }

    public void OnRelease()
    {
        if (inRange)
        {
            transform.DOMove(task.FinalTarget.position, 0.2f);
            transform.SetParent(task.transform.parent);
            transform.DORotateQuaternion(task.FinalTarget.rotation, 0.2f);

            Destroy(GetComponent<Throwable>());
            Destroy(GetComponent<InteractableHoverEvents>());
            Destroy(GetComponent<Interactable>());
            Destroy(GetComponent<VelocityEstimator>());
            Destroy(GetComponent<SteamVR_Skeleton_Poser>());

            task.PreviouslyUsedObject = gameObject;
            materialRenderer.material = standartMaterial;
            completed = true;
            task.OnTaskComplete?.Invoke(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (inRange && !completed)
        {
            materialRenderer.material = inrangeMaterial;
        }
    }
}
