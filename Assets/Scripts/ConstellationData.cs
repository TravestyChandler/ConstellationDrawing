using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

[System.Serializable]
public class ConstellationData : ScriptableObject
{
    [fsProperty]
    public List<Vector3> positions;

    public ConstellationData()
    {
        positions = new List<Vector3>();
    }

}
