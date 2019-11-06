using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSequence : MonoBehaviour
{
    [SerializeField] private List<BaseTask> tasksToComplete;

    public Action<TaskSequence> OnCompleted;

    public void LoadSequence()
    {
        foreach (BaseTask task in tasksToComplete)
        {
            task.Sequence = this;
            task.TryStartTask();
        }
    }

    public void TaskCompleted()
    {
        bool completedAllTasks = true;
        for (int i = 0; i < tasksToComplete.Count; i++)
        {
            if(tasksToComplete[i].Completed == false)
            {
                completedAllTasks = false;
                foreach (BaseTask task in tasksToComplete)
                {
                    task.TryStartTask();
                }
                break;
            }
        }

        if (completedAllTasks)
        {
            Debug.Log("[TASK] Finished task Sequence");
            OnCompleted?.Invoke(this);
        }
    }
}