using System.Collections.Generic;
using UnityEngine;
using ClosetBuilder;
using System.Collections;

public class SequenceManager : Singleton<SequenceManager>
{
    [SerializeField] private SequenceSwitcher sequenceSwitcher;
    [SerializeField] private GameObject sequenceHolder;

    private Sequence[] playableSequences;

    private int sequenceIndex;

    private int totalSequenceCount;
    private int currentSequencesCompleted;

    void Start()
    {
        playableSequences = sequenceHolder.GetComponentsInChildren<Sequence>();
        foreach (Sequence sequence in playableSequences)
        {
            sequence.OnSequenceComplete += BuildSequenceCompleted;
        }

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        sequenceSwitcher.SwitchSequence(playableSequences[sequenceIndex].BuildData, playableSequences[sequenceIndex]);
    }

    public void BuildSequenceCompleted()
    {
        sequenceIndex++;

        if(sequenceIndex == playableSequences.Length)
        {
            Debug.Log("Build finished");
        }
        else
        {
            sequenceSwitcher.SwitchSequence(playableSequences[sequenceIndex].BuildData, playableSequences[sequenceIndex]);
        }
    }
}
