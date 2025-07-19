using StudentWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject losemenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private CharacterManager characterManager;
     public LevelManager levelManager;
     public int lives;
     private UI ui;

    public bool gameplay;
    private bool paused;
    public bool lockCursor;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Pause();
        }
        lockCursor = !paused && gameplay ? true : false;

        if (paused)
        {
            PlayerLocatorSingleton.Instance
                .GetComponent<PlayerController>().playerInput.enabled = false;
        }
        else
        {
            PlayerLocatorSingleton.Instance
                .GetComponent<PlayerController>().playerInput.enabled = true;
        }

    }
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        characterManager.SpawnCharacter();
        ui = PlayerLocatorSingleton.Instance.GetComponent<UI>();
    }
    public void InitializeGame()
    {
        lives = 3;
        ui.updateLivesCount(lives);
        characterManager.SpawnCharacter();
        levelManager.UnloadScene("MainMenu");
        levelManager.LoadLevelAdditively("World");
        gameplay = true;

    }


    public void RestartLevel()
    {
      
        ui.updateLivesCount(lives);
        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("World");
        Pause();

        winMenu.SetActive(false);
        losemenu.SetActive(false);

        lives = 3;
        ui.updateLivesCount(lives);
        StartCoroutine(enableTimer());
       
    }

    //Activates the players OnEnablefunction 
    IEnumerator enableTimer()
    {
        gameplay = false;
      yield return new WaitForSecondsRealtime(0.01f);
        gameplay = true;
        
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            losemenu.SetActive(true);
            showCurrentTime();
            paused = true;
        }
        else
        {
            losemenu.SetActive(false);
            
            PlayerLocatorSingleton.Instance.GetComponent<PlayerController>().SnapBackToGround();
            ui.updateLivesCount(lives);
        }
    }

    
    public void MainMenu()
    {
        winMenu.SetActive(false);
        losemenu.SetActive(false);
        gameplay = false;
        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("MainMenu");
    }
    public void GameWinSequence()
    {
        showCurrentTime();

       
        
        winMenu.SetActive(true);

        paused = true;
        Time.timeScale = 0;
    }

    public void Pause()
    {

        showCurrentTime();
        

        pauseMenu.SetActive(!paused);

        Time.timeScale = paused ? 1 : 0;
        paused = !paused;
    }

    private void showCurrentTime()
    {
        timeText.text = ui.gameTimer.ToString("F2");
        timeText.gameObject.SetActive(!paused);

        ui.timerPaused = !paused;
    }

}
