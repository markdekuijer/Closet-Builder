using System;
using UnityEngine;

namespace ClosetBuilder
{
    public class Sequence : MonoBehaviour
    {
        public BuildSequenceData BuildData => buildData;

        public Action OnSequenceComplete;
        public Sprite SequenceExplainer;

        [SerializeField] private BuildSequenceData buildData;
        [SerializeField] private GameObject screwSet;

        private TaskSequence[] taskSequences;
        private int totalSequences;
        private int currentCompletedSequences;


        public void StartLoadSequence()
        {
            screwSet.transform.position = new Vector3(2.59f, 0.761f, 0.324f);
            taskSequences = GetComponentsInChildren<TaskSequence>();
            totalSequences = taskSequences.Length;
            foreach (TaskSequence tasks in taskSequences)
            {
                tasks.OnCompleted += TaskSequenceComplete;
                tasks.LoadSequence();
            }
        }

        private void TaskSequenceComplete(TaskSequence completedSequence)
        {
            currentCompletedSequences++;

            completedSequence.OnCompleted -= TaskSequenceComplete;

            if (currentCompletedSequences >= totalSequences)
            {
                OnSequenceComplete?.Invoke();
            }
        }
    }
}