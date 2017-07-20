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
    public SpriteRenderer ConstImage;
    public int Score = 0;
    public float maxCheckDistance = 0.25f;

    public AudioClip success;
    public AudioClip failure;
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
        UIManager.Instance.SetScoreText(0);

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
            ending = true;
            StartCoroutine(EndingRoutine());
            bool victory = CheckScore();
            if (victory)
            {
                UIManager.Instance.RoundVictory();
                Score++;
                PlaySFX(success);
                UIManager.Instance.SetScoreText(Score);
                currentPhase = GamePhase.Starting;
            }
            else
            {
                UIManager.Instance.RoundLoss();
                PlaySFX(failure);
                Score = 0;
                currentPhase = GamePhase.GameOver;
            }
            foreach(Transform trans in pointParent)
            {
                Destroy(trans.gameObject);
            }
        }
        else if (currentPhase == GamePhase.Starting && !ending)
        {
            gameTimer = InitialTimer;
            if (roundStartTime <= 0f)
            {
                UIManager.Instance.SetRoundStartTimer(0);
                currentPhase = GamePhase.Running;
                roundStartTime = InitialRoundStartTimer;
                currentConstellation = SelectConstellation();
                foreach(Vector3 p in currentConstellation.positions)
                {
                    GameObject game = GameObject.Instantiate(pointPrefab, pointParent);
                    game.transform.position = p;
                }
                ConstImage.sprite = currentConstellation.constellationImage;
                UIManager.Instance.RoundStart();
                UIManager.Instance.SetConstellationData(currentConstellation);
            }
            else
            {
                roundStartTime -= Time.deltaTime;
                UIManager.Instance.SetRoundStartTimer(roundStartTime);
            }
            UIManager.Instance.SetScoreText(Score);
        }
        else if(currentPhase == GamePhase.GameOver && !ending)
        {

        }
	}

    public ConstellationData SelectConstellation()
    {
        List<ConstellationData> currentDifficultyConstellations = new List<ConstellationData>();
        if (Score < 5)
        {
            foreach (ConstellationData con in possibleConstellations)
            {
                if (con.difficulty == ConstellationData.Difficulty.Easy)
                {
                    currentDifficultyConstellations.Add(con);
                }
            }
        }
        else if (Score < 10)
        {
            foreach (ConstellationData con in possibleConstellations)
            {
                if (con.difficulty == ConstellationData.Difficulty.Easy || con.difficulty == ConstellationData.Difficulty.Medium)
                {
                    currentDifficultyConstellations.Add(con);
                }
            }
        }
        else {
            foreach (ConstellationData con in possibleConstellations)
            {
                if (con.difficulty == ConstellationData.Difficulty.Medium || con.difficulty == ConstellationData.Difficulty.Hard)
                {
                    currentDifficultyConstellations.Add(con);
                }
            }
        }
        return currentDifficultyConstellations[Random.Range(0, currentDifficultyConstellations.Count)];
    }
    public bool ending = false;
    public IEnumerator EndingRoutine()
    {
        yield return StartCoroutine(FadeInConstellation());
        UIManager.Instance.HideSuccess();
        yield return new WaitForSeconds(2f);
        UIManager.Instance.NextRoundStart();
        ending = false;
        yield return StartCoroutine(FadeOutConstellation());
        PlayerController.Instance.ResetPlayerController();

    }
    public IEnumerator FadeInConstellation()
    {
        float fadeInTime = 0.75f;
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            yield return null;
            float val = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            Color col = ConstImage.color;
            col.a = val;
            ConstImage.color = col;
        }
    }

    public IEnumerator FadeOutConstellation()
    {
        float fadeOutTime = 0.75f;
        float timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            yield return null;
            float val = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            Color col = ConstImage.color;
            col.a = val;
            ConstImage.color = col;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        GameObject game = new GameObject();
        game.name = clip.name;
        AudioSource s = game.AddComponent<AudioSource>();
        s.PlayOneShot(clip);
        Destroy(game, 3f);
    }
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
