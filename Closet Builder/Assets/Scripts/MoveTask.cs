using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MoveTask : BaseTask
{
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private LinearMapping completion;
    [SerializeField, Range(0,1)] private float targetVal;
    [SerializeField] private Transform target;
    [SerializeField] private Transform start;
    [SerializeField] private Vector3 targetOffset;

    private void Awake()
    {
        objectToMove.GetComponent<InteractableHoverEvents>().Activated = false;
        objectToMove.GetComponent<Interactable>().onDetachedFromHand += CheckCompletion;
        start.transform.position = objectToMove.transform.position;
        target.transform.position = objectToMove.transform.position + targetOffset;
    }

    protected override void StartTask()
    {
        base.StartTask();
        objectToMove.GetComponent<InteractableHoverEvents>().Activated = true;
        objectToMove.GetComponent<Interactable>().enabled = true;
        objectToMove.AddComponent<LinearDrive>().Initialize(start, target, completion);
    }

    public void CheckCompletion(Hand hand)
    {
        if(completion.value == targetVal)
        {
            Destroy(objectToMove.GetComponent<InteractableHoverEvents>());
            Destroy(objectToMove.GetComponent<LinearDrive>());
            Destroy(objectToMove.GetComponent<Interactable>());

            objectToMove.GetComponent<Interactable>().onDetachedFromHand -= CheckCompletion;
            OnTaskComplete?.Invoke(objectToMove);
        }
    }
}
