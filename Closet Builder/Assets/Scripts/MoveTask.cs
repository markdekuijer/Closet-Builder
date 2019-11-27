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
    [SerializeField] private bool dontDestroyStuff;

    private void Awake()
    {
        objectToMove.GetComponent<InteractableHoverEvents>().Activated = false;
        objectToMove.GetComponent<Interactable>().onDetachedFromHand += CheckCompletion;
        objectToMove.GetComponent<BoxCollider>().enabled = false;
    }

    protected override void StartTask()
    {
        base.StartTask();
        start.transform.position = objectToMove.transform.localPosition;
        target.transform.position = objectToMove.transform.localPosition + targetOffset;
        objectToMove.GetComponent<InteractableHoverEvents>().Activated = true;
        objectToMove.GetComponent<Interactable>().enabled = true;
        objectToMove.GetComponent<BoxCollider>().enabled = true;
        objectToMove.AddComponent<LinearDrive>().Initialize(start, target, completion);
    }

    public void CheckCompletion(Hand hand)
    {
        if(completion.value == targetVal)
        {
            Destroy(objectToMove.GetComponent<LinearDrive>());
            if (!dontDestroyStuff)
            {
                Destroy(objectToMove.GetComponent<InteractableHoverEvents>());
                Destroy(objectToMove.GetComponent<Interactable>());
                objectToMove.GetComponent<Interactable>().onDetachedFromHand -= CheckCompletion;
            }

            objectToMove.GetComponent<BoxCollider>().enabled = false;
            OnTaskComplete?.Invoke(objectToMove);
        }
    }
}
