using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClosetBuilder;
using System;
using Valve.VR;

public class SequenceSwitcher : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> explainers;
    [SerializeField] private Transform searchTransform;
    [SerializeField] private AudioSource switcher;
    public void SwitchSequence(BuildSequenceData BuildSequence, Sequence nextSequence)
    {
        StartCoroutine(SwitchSequenceRoutine(BuildSequence, nextSequence));
    }

    private IEnumerator SwitchSequenceRoutine(BuildSequenceData BuildSequence, Sequence nextSequence)
    {
        SteamVR_Fade.View(Color.black, 1);
        CameraFader.Instance.SwitchSequence();
        yield return new WaitForSeconds(.5f);
        switcher.Play();
        yield return new WaitForSeconds(1.5f);
        explainers.ForEach(x => x.sprite = nextSequence.SequenceExplainer);
        LoadScriptableObjectSequence(BuildSequence);
        nextSequence.StartLoadSequence();
        SteamVR_Fade.View(Color.clear, 1);
    }

    public int LoadScriptableObjectSequence(BuildSequenceData BuildSequence)
    {
        int totalTasks = 0;

        foreach (Transform childTransform in searchTransform)
        {
            SequenceComponentData sequenceData = BuildSequence.GetData(childTransform.gameObject.name);
            if (sequenceData != null)
            {
                if(transform.GetComponent<TaskSequence>() != null)
                {
                    totalTasks++;
                }

                childTransform.localPosition = sequenceData.Position;
                childTransform.localRotation = Quaternion.Euler(sequenceData.RotationEuler);
                childTransform.localScale = sequenceData.Scale;
            }
            else
            {
                Debug.LogWarning($"Unable to find data for {childTransform.name}. Please remap the data object");
            }
        }

        return totalTasks;
    }
}
