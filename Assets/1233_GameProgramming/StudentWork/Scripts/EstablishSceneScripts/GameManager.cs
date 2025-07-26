using StudentWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string _currentScene;

    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private GameObject Losemenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private CharacterManager CharacterManager;
     public LevelManager LevelManager;
     public int Lives;
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

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CharacterManager.SpawnCharacter();
        ui = PlayerLocatorSingleton.Instance.GetComponent<UI>();
    }
    public void InitializeGame(string levelName)
    {
       _currentScene = levelName;
        Lives = 3;
        ui.updateLivesCount(Lives);
        CharacterManager.SpawnCharacter();
        LevelManager.UnloadScene("MainMenu");
        LevelManager.LoadLevelAdditively(levelName);
       
        gameplay = true;
        
    }


    public void RestartLevel()
    {

        

        ui.updateLivesCount(Lives);
        LevelManager.UnloadScene(_currentScene);
        LevelManager.LoadLevelAdditively(_currentScene);
        Pause();

        WinMenu.SetActive(false);
        Losemenu.SetActive(false);

        Lives = 3;
        ui.updateLivesCount(Lives);
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
        Lives--;

        if (Lives <= 0)
        {
            Losemenu.SetActive(true);
            showCurrentTime();
            paused = true;
        }
        else
        {
            Losemenu.SetActive(false);
            
            PlayerLocatorSingleton.Instance.GetComponent<PlayerController>().SnapBackToGround();
            ui.updateLivesCount(Lives);
        }
    }

    
    public void MainMenu()
    {
        WinMenu.SetActive(false);
        Losemenu.SetActive(false);
        gameplay = false;
        LevelManager.UnloadScene(_currentScene);
        LevelManager.LoadLevelAdditively("MainMenu");
    }
    public void GameWinSequence()
    {
        showCurrentTime();

       
        
        WinMenu.SetActive(true);

        paused = true;
        Time.timeScale = 0;
    }

    public void Pause()
    {

        showCurrentTime();
        

        PauseMenu.SetActive(!paused);

        Time.timeScale = paused ? 1 : 0;
        paused = !paused;
    }

    private void showCurrentTime()
    {
        TimeText.text = ui.gameTimer.ToString("F2");
        TimeText.gameObject.SetActive(!paused);

        ui.timerPaused = !paused;
    }

}
