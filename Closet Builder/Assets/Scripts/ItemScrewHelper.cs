using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ItemScrewHelper : MonoBehaviour
{
    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;

    [SerializeField] private Material inrangeMaterial;

    private Material standartMaterial;
    private Renderer materialRenderer;
    private bool inRange;
    private bool inPlace;
    private bool grabbed;
    private ScrewTask task;
    private Vector3 targetPosition;
    private float passedDelta;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioSource placesound;

    private void Start()
    {
        materialRenderer = GetComponentInChildren<Renderer>();
    }

    public void ForceInRangeMaterial(bool shouldForce, ScrewTask placeTask)
    {
        if (inPlace)
            return;

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
        grabbed = true;
        if (standartMaterial == null)
        {
            standartMaterial = materialRenderer.material;
        }

        if (inPlace)
        {
            //startPosFromPreviousTask = task.ObjectToScrew.transform.position;

        }
    }

    public void OnRelease()
    {
        if (!inPlace)
        {
            if (inRange)
            {
                placesound.Play();
                transform.DOMove(task.TargetTransform.position, 0.2f);
                transform.DORotateQuaternion(task.TargetTransform.rotation, 0.2f).OnComplete(() => {
                    if (task.ObjectToScrew != null)
                    {
                        task.ObjectToScrew.transform.SetParent(transform);// = GetTargetPos(passedPercentage);
                    }
                });
                transform.SetParent(task.transform.parent);

                task.OnSuccesfullyPlaced?.Invoke();
                inPlace = true;
                targetPosition = task.TargetTransform.position;
                lockRotationVector = task.TargetTransform.rotation.eulerAngles;
            }
        }
        else
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(lockRotationVector.x, lockRotationVector.y, transform.rotation.z);
        }

        grabbed = false;
    }

    private Vector3 lockRotationVector;

    private float previousDelta;

    private Vector3 startPosFromPreviousTask;

    private void LateUpdate()
    {
        if (inRange)
        {
            materialRenderer.material = inrangeMaterial;
        }

        if (grabbed && inPlace)
        {
            if (m_MovePress.GetStateDown(SteamVR_Input_Sources.Any))
            {
                sound.Play();
                previousDelta = m_MoveValue.axis.x;
            }

            if (m_MovePress.GetStateUp(SteamVR_Input_Sources.Any))
            {
                sound.Stop();
            }

            float diff = 0;
            if (m_MovePress.GetState(SteamVR_Input_Sources.Any))
            {
                diff = Mathf.Abs(previousDelta - m_MoveValue.axis.x);
                previousDelta = m_MoveValue.axis.x;
                passedDelta += diff;
            }

            if(passedDelta < 0) { passedDelta = 0; }
            if(passedDelta > task.RequiredPassedDelta) { passedDelta = task.RequiredPassedDelta; }

            float passedPercentage;
            if(passedDelta != 0)
            {
                passedPercentage = passedDelta / task.RequiredPassedDelta;
            }
            else
            {
                passedPercentage = 0;
            }

            transform.position = Vector3.Lerp(targetPosition, task.FinalTargetPos, passedPercentage);
            transform.rotation = Quaternion.Euler(lockRotationVector.x, lockRotationVector.y, Mathf.Lerp(0, task.TotalRotation, passedPercentage));

            if (passedDelta == task.RequiredPassedDelta)
            {
                task.ObjectToScrew.transform.SetParent(null);
                task.OnTaskComplete?.Invoke(task.ObjectToScrew);
                inRange = false;
                grabbed = false;
                inPlace = false;
                targetPosition = Vector3.zero;
                passedDelta = 0;
                task = null;
                materialRenderer.material = standartMaterial;
                sound.Stop();
            }
        }
    }
}
