using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLineRend : MonoBehaviour {
    public LineRenderer lr;
    public Material mat;

    public Vector2 animRate = new Vector2(1.0f, 0.0f);
    public Vector2 uvOffset = Vector2.zero;
	// Use this for initialization
	void Start () {
        mat = lr.material;
	}
	
	// Update is called once per frame
	void Update () {
        uvOffset += (animRate * Time.deltaTime);
        mat.SetTextureOffset("_MainTex", uvOffset);
	}
}
