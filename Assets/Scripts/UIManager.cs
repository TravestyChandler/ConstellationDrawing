using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    [SerializeField]
    private Text gameTimer;

    [SerializeField]
    private Text roundStartTimer;

    public RectTransform successPanel;

    public RectTransform failurePanel;

    public Slider inkSlider;

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

    public void GameRestart()
    {
        GameManager.Instance.currentPhase = GameManager.GamePhase.Starting;
        gameTimer.enabled = false;
        roundStartTimer.enabled = true;
        StartCoroutine(HideFailurePanel());
    }
    public void SetSliderValue(float val)
    {
        inkSlider.value = val;
    }
    public void SetGameTimer(float time)
    {
        gameTimer.text = time.ToString("0.00");
    }

    public void SetRoundStartTimer(float time)
    {
        roundStartTimer.text = "Next round in: " + time.ToString("0.00");
    }
    
    public void GameStart()
    {
        roundStartTimer.enabled = true;
        gameTimer.enabled = false;
        successPanel.gameObject.SetActive(false);
        failurePanel.gameObject.SetActive(false);
    }

    public void RoundVictory()
    {
        gameTimer.enabled = false;
        roundStartTimer.enabled = true;
        StartCoroutine(ShowSuccessPanel());
    }

    public void RoundStart()
    {
        gameTimer.enabled = true;
        roundStartTimer.enabled = false;
        StartCoroutine(HideSuccessPanel());
    }
    public IEnumerator ShowSuccessPanel()
    {
        successPanel.gameObject.SetActive(true);
        float timer = 0f;
        while(timer < 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
            successPanel.localScale = Vector3.one * Mathf.Lerp(0f, 1f, timer / 0.25f);
        }
    }
    public IEnumerator HideSuccessPanel()
    {
        float startScale = successPanel.localScale.x;
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
            successPanel.localScale = Vector3.one * Mathf.Lerp(startScale, 0f, timer / 0.25f);
        }
        successPanel.gameObject.SetActive(false);

    }

    public IEnumerator ShowFailurePanel()
    {
        failurePanel.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
            failurePanel.localScale = Vector3.one * Mathf.Lerp(0f, 1f, timer / 0.25f);
        }
    }
    public IEnumerator HideFailurePanel()
    {
        float startScale = failurePanel.localScale.x;
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
            failurePanel.localScale = Vector3.one * Mathf.Lerp(startScale, 0f, timer / 0.25f);
        }
        failurePanel.gameObject.SetActive(false);

    }
    public void RoundLoss()
    {
        gameTimer.enabled = false;
        StartCoroutine(ShowFailurePanel());
    }
}
