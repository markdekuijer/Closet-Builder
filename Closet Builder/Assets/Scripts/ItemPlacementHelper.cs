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
    [SerializeField] private float inverseLenght;

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
            transform.SetParent(task.transform.parent);
            if (!inversed)
            {
                transform.DOMove(task.FinalTarget.position, 0.2f);
                transform.DORotate(task.FinalTarget.rotation.eulerAngles, 0.2f);
            }
            else
            {
                Vector3 targetRot = ReverseObject();
                transform.DORotate(targetRot, 0.2f);
                Vector3 difVec = task.FinalTarget.position - transform.position;
                Vector3 vecToAdd = difVec.normalized * inverseLenght;
                transform.position += vecToAdd;
                transform.DOMove(task.FinalTarget.position, 0.2f);
            }

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

    public Vector3 ReverseObject()
    {
        if (task.XYZReverse.x == 1)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        if (task.XYZReverse.y == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

        }
        if (task.XYZReverse.z == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
        }

        if (task.RotationReverse.x == 1)
        {
            return new Vector3(task.FinalTarget.transform.rotation.eulerAngles.x + 180, task.FinalTarget.transform.rotation.eulerAngles.y, task.FinalTarget.transform.rotation.eulerAngles.z);
        }
        if (task.RotationReverse.y == 1)
        {
            return new Vector3(task.FinalTarget.transform.rotation.eulerAngles.x, task.FinalTarget.transform.rotation.eulerAngles.y + 180, task.FinalTarget.transform.rotation.eulerAngles.z);
        }
        if (task.RotationReverse.z == 1)
        {
            return new Vector3(task.FinalTarget.transform.rotation.eulerAngles.x, task.FinalTarget.transform.rotation.eulerAngles.y, task.FinalTarget.transform.rotation.eulerAngles.z + 180);
        }

        return new Vector3(task.FinalTarget.transform.rotation.eulerAngles.x, task.FinalTarget.transform.rotation.eulerAngles.y + 180, task.FinalTarget.transform.rotation.eulerAngles.z);
    }

    private void LateUpdate()
    {
        if (inRange && !completed)
        {
            materialRenderer.material = inrangeMaterial;
        }
    }
}
