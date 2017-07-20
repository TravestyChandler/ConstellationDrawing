using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

[System.Serializable]
public class ConstellationData : ScriptableObject
{

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }


    [fsProperty]
    public string constellationName;

    [fsProperty]
    public Sprite constellationImage;

    [fsProperty]
    public Difficulty difficulty;

    [fsProperty]
    public List<Vector3> positions;

    public ConstellationData()
    {
        positions = new List<Vector3>();
    }

}
