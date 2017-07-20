using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    public Camera cam;
    public Vector3 startPos;
    public Vector3 currentPos;
    public float minDist = 0.1f;
    public LineRenderer lr;
    public int maxInk = 50;
    public int currentInk;
    public int inkThisRound;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }
    void Start () {
        cam = Camera.main;
        inkThisRound = maxInk;
        currentInk = inkThisRound;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.currentPhase == GameManager.GamePhase.Running)
        {
            inkThisRound = Mathf.Clamp(maxInk - GameManager.Instance.Score, 70, maxInk);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(inkThisRound);
                ClearLineRend();
                lr.positionCount = 1;
                startPos = cam.ScreenToWorldPoint(Input.mousePosition);
                startPos.z += 10;
                lr.SetPosition(0, startPos);
                currentPos = startPos;
                currentInk--;
                UIManager.Instance.SetSliderValue((float)currentInk / (float)inkThisRound);
            }
            else if (Input.GetMouseButton(0) && currentInk > 0)
            {
                //Debug.Log(Input.mousePosition);
                currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
                currentPos.z += 10;

                if (Vector3.Distance(lr.GetPosition(lr.positionCount - 1), currentPos) > minDist)
                {
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, currentPos);
                    currentInk--;
                    UIManager.Instance.SetSliderValue((float)currentInk / (float)inkThisRound);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
                currentPos.z += 10;
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, currentPos);
                currentInk = inkThisRound;
                UIManager.Instance.SetSliderValue((float)currentInk / (float)inkThisRound);
            }  
        }
        else
        {
            currentInk = inkThisRound;
        }
    }

    public void ClearLineRend()
    {
        lr.positionCount = 0;
    }

    public void ResetPlayerController()
    {
        currentInk = inkThisRound;
        UIManager.Instance.SetSliderValue((float)currentInk / (float)inkThisRound);
        ClearLineRend();
    }
}
