using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public enum GamePhase
    {
        Starting,
        Running,
        Ending,
        GameOver,
        None
    }
    public GamePhase currentPhase = GamePhase.Starting;
    public float InitialTimer = 7f;
    private float gameTimer = 7f;
    public float InitialRoundStartTimer = 5f;
    private float roundStartTime = 5f;

    public List<ConstellationData> possibleConstellations;
    public Transform pointParent;
    public GameObject pointPrefab;
    public ConstellationData currentConstellation;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }


	// Use this for initialization
	void Start () {
        gameTimer = InitialTimer;
        roundStartTime = InitialRoundStartTimer;
        currentPhase = GamePhase.Starting;
        UIManager.Instance.GameStart();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentPhase == GamePhase.Running)
        {
            if (gameTimer <= 0)
            {
                currentPhase = GamePhase.Ending;
                UIManager.Instance.SetGameTimer(0);
            }
            else {
                gameTimer -= Time.deltaTime;
                UIManager.Instance.SetGameTimer(gameTimer);
            }
        }
        else if (currentPhase == GamePhase.Ending)
        {
            bool victory = CheckScore();
            if (victory)
            {
                UIManager.Instance.RoundVictory();
                currentPhase = GamePhase.Starting;
            }
            else
            {
                UIManager.Instance.RoundLoss();
                currentPhase = GamePhase.GameOver;
            }
            foreach(Transform trans in pointParent)
            {
                Destroy(trans.gameObject);
            }
            PlayerController.Instance.ResetPlayerController();
        }
        else if (currentPhase == GamePhase.Starting)
        {
            gameTimer = InitialTimer;
            if (roundStartTime <= 0f)
            {
                UIManager.Instance.SetRoundStartTimer(0);
                currentPhase = GamePhase.Running;
                roundStartTime = InitialRoundStartTimer;
                currentConstellation = possibleConstellations[Random.Range(0, possibleConstellations.Count)];
                foreach(Vector3 p in currentConstellation.positions)
                {
                    GameObject game = GameObject.Instantiate(pointPrefab, pointParent);
                    game.transform.position = p;
                }
                UIManager.Instance.RoundStart();
            }
            else
            {
                roundStartTime -= Time.deltaTime;
                UIManager.Instance.SetRoundStartTimer(roundStartTime);
            }
        }
        else if(currentPhase == GamePhase.GameOver)
        {

        }
	}

    public float maxCheckDistance = 0.25f;
    public bool CheckScore()
    {
        LineRenderer lr = PlayerController.Instance.lr;
        Vector3[] points = new Vector3[lr.positionCount];
        lr.GetPositions(points);
        bool matchFound = false;

        foreach (Vector3 p in currentConstellation.positions)
        {
            matchFound = false;
            foreach(Vector3 pos in points)
            {
                if(Vector2.Distance(pos, p) < maxCheckDistance)
                {
                    matchFound = true;
                }
            }
            if (matchFound == false)
            {
                return false;
            }
        }
        return true;
    }
}
