using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "Sequences")]
public class BuildSequenceData : ScriptableObject
{
    public List<SequenceComponentData> transformData;

    public SequenceComponentData GetData(string ID)
    {
        return transformData.Find(x => x.ID == ID);
    }
}

[System.Serializable]
public class SequenceComponentData
{
    public string ID;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 RotationEuler;
}