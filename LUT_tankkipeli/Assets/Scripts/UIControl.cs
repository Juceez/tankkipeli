using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public Text scoreText;
    public Text healthText;
    public Text livesText;
    public Text endScore;

    public GameObject pauseMenu;
    public GameObject respawnScreen;
    public GameObject endScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    public void SetScore(float score)
    {
        scoreText.text = "Score: " + score;
    }
    
    public void SetHealth(float current, float max)
    {
        healthText.text = "Health: " + current + "/" + max;
    }
    
    public void SetLives(int current, int max)
    {
        livesText.text = "Lives: " + current + "/" + max;
    }

    public void TogglePause()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else if(!endScreen.activeInHierarchy && !respawnScreen.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void showRespawn()
    {
        respawnScreen.SetActive(true);
    }

    public void hideRespawn()
    {
        respawnScreen.SetActive(false);
    }

    public void ShowEndScreen(float score)
    {
        endScore.text = "Score: " + score;
        endScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false;
#else        
        Application.Quit();
#endif
    }
    
}
