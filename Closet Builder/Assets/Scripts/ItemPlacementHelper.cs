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
    private bool inversed;

    private void Start()
    {
        materialRenderer = GetComponentInChildren<Renderer>();
    }

    public void ForceInRangeMaterial(bool shouldForce, PlaceTask placeTask, bool inversed = false)
    {
        inRange = shouldForce;
        this.inversed = inversed;

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
            if (!inversed)
            {
                transform.DORotateQuaternion(task.FinalTarget.rotation, 0.2f);
            }
            else
            {
                ReverseObject();
            }


            Destroy(GetComponent<Throwable>());
            Destroy(GetComponent<InteractableHoverEvents>());
            Destroy(GetComponent<Interactable>());
            Destroy(GetComponent<VelocityEstimator>());
            Destroy(GetComponent<SteamVR_Skeleton_Poser>());

            task.PreviouslyUsedObject = gameObject;
            materialRenderer.material = standartMaterial;
            completed = true;
            inversed = false;
            task.OnTaskComplete?.Invoke(gameObject);
        }
    }

    public void ReverseObject()
    {
        if (task.XYZReverse.x == 1)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (task.XYZReverse.y == 1)
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        if (task.XYZReverse.z == 1)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    private void LateUpdate()
    {
        if (inRange && !completed)
        {
            materialRenderer.material = inrangeMaterial;
        }
    }
}
