using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CreateConstellationData : MonoBehaviour {
    [MenuItem("Assets/Create/Create ConstellationData")]
    public static void CreateLevelData()
    {
        ConstellationData asset = ScriptableObject.CreateInstance<ConstellationData>();  //scriptable object
        AssetDatabase.CreateAsset(asset, "Assets/Constellations/ConstellationDataNew.asset");
        AssetDatabase.SaveAssets();
        Selection.activeObject = asset;
    }
}
