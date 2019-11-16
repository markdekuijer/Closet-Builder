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

    private float totalDifferenceFromPreviousTask;
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
                previousDelta = m_MoveValue.axis.x;
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

            if (totalDifferenceFromPreviousTask == 0)
            {
                totalDifferenceFromPreviousTask = Vector3.Distance(targetPosition, task.PlaceTask.FinalTarget.position);
                Debug.Log("Found new distance!" + totalDifferenceFromPreviousTask);
            }

            //previously here

            if (passedDelta == task.RequiredPassedDelta)
            {
                task.ObjectToScrew.transform.SetParent(null);
                task.OnTaskComplete?.Invoke(task.ObjectToScrew);
                totalDifferenceFromPreviousTask = 0f;
                inRange = false;
                grabbed = false;
                inPlace = false;
                targetPosition = Vector3.zero;
                passedDelta = 0;
                task = null;
            }
        }
    }

    public Vector3 GetTargetPos(float t)
    {
        switch (task.ScrewAxis)
        {
            case TargetAxis.x:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos + new Vector3(totalDifferenceFromPreviousTask, 0, 0), t);
            case TargetAxis.y:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos + new Vector3(0, totalDifferenceFromPreviousTask, 0), t);
            case TargetAxis.z:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos + new Vector3(0, 0, totalDifferenceFromPreviousTask), t);
            case TargetAxis.minx:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos - new Vector3(totalDifferenceFromPreviousTask, 0, 0), t);
            case TargetAxis.miny:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos - new Vector3(0, totalDifferenceFromPreviousTask, 0), t);
            case TargetAxis.minz:
                return Vector3.Lerp(startPosFromPreviousTask, task.FinalTargetPos - new Vector3(0, 0, totalDifferenceFromPreviousTask), t);
        }

        return Vector3.zero;
    }
}
