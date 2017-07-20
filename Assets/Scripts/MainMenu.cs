using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Application.CaptureScreenshot("starmapperscreenshot.png");
        }
    }
}
