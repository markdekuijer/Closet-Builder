using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ItemPlacementHelper : MonoBehaviour
{
    [SerializeField] private Material inrangeMaterial;
    [SerializeField] private float inverseLenght;

    private Material standartMaterial;
    private List<Renderer> materialRenderer;
    private bool inRange;
    private bool completed;
    private PlaceTask task;
    private bool inversed;
    private AudioSource placedAudio;

    private void Start()
    {
        materialRenderer = GetComponentsInChildren<Renderer>().ToList();
        placedAudio = GetComponent<AudioSource>();
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
            foreach (Renderer renderer in materialRenderer)
            {
                renderer.material = standartMaterial;
            }
        }
    }

    public void OnGrab()
    {
        if(standartMaterial == null)
        {
            standartMaterial = materialRenderer[0].material;
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

            foreach (Transform t in transform)
            {
                if (t.CompareTag("SecretObj"))
                {
                    task.SecretObject = t.gameObject;
                    break;
                }
            }

            foreach (Renderer renderer in materialRenderer)
            {
                renderer.material = standartMaterial;
            }
            completed = true;
            task.OnTaskComplete?.Invoke(gameObject);
            placedAudio.Play();
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
            foreach (Renderer renderer in materialRenderer)
            {
                renderer.material = inrangeMaterial;
            }
        }
    }
}
