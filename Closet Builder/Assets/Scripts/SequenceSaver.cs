using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceSaver : MonoBehaviour
{
    [SerializeField] private BuildSequenceData sequence;
    [SerializeField] private Transform target; 

    [ContextMenu("Populate Sequence")]
    void CreateScriptableObjectSequence()
    {
        sequence.transformData = new List<SequenceComponentData>();
        foreach (Transform childTransform in target)
        {
            Debug.Log("Found " + childTransform.gameObject.name);
            SequenceComponentData st = new SequenceComponentData();

            st.ID = childTransform.gameObject.name;
            st.Position = childTransform.localPosition;
            st.RotationEuler = childTransform.localRotation.eulerAngles;
            st.Scale = childTransform.localScale;

            sequence.transformData.Add(st);
        }
    }

    [ContextMenu("Load Sequence")]
    void LoadScriptableObjectSequence()
    {
        foreach (Transform childTransform in target)
        {
            SequenceComponentData st = sequence.GetData(childTransform.gameObject.name);
            

            if(st != null)
            {
                childTransform.localPosition = st.Position;
                childTransform.localRotation = Quaternion.Euler(st.RotationEuler);
                childTransform.localScale = st.Scale;
            }
            else
            {
                Debug.LogWarning($"Unable to find data for {childTransform.name}. Please remap the data object");
            }
        }
    }
}
