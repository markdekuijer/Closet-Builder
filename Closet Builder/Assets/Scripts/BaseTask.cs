using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BaseTask : MonoBehaviour
{
    public bool Completed { get; private set; }
    public bool Running { get; private set; }

    public TaskSequence Sequence;
    public Action<GameObject> OnTaskComplete;

    [SerializeField] protected GameObject highlightedIndicator;
    [SerializeField] private List<BaseTask> tasksRequiredToStart;
    [SerializeField] private Transform parentToSnapOn;
         
    private AudioSource taskCompletedAudio;

    public virtual void TryStartTask()
    {
        if(Completed || Running)
        {
            return;
        }

        if(tasksRequiredToStart != null && tasksRequiredToStart.Count != 0)
        {
            foreach (BaseTask taskToCheck in tasksRequiredToStart)
            {
                if(taskToCheck.Completed == false)
                {
                    return;
                }
            }
        }

        StartTask();
    }

    protected virtual void StartTask()
    {
        Debug.Log("[TASK] Started new task: " + this.GetType());
        taskCompletedAudio = GetComponent<AudioSource>();
        OnTaskComplete += TaskComplete;
        highlightedIndicator.gameObject.SetActive(true);
        Running = true;
    }

    protected virtual void TaskComplete(GameObject usedObject)
    {
        Running = false;
        Completed = true;
        Debug.Log("[TASK] Finished task: " + this.GetType());
        //taskCompletedAudio.Play();
        OnTaskComplete -= TaskComplete;
        highlightedIndicator.gameObject.SetActive(false);
        Sequence.TaskCompleted();

        if(parentToSnapOn != null && usedObject != null)
        {
            usedObject.transform.SetParent(parentToSnapOn, true);
        }
    }
}
