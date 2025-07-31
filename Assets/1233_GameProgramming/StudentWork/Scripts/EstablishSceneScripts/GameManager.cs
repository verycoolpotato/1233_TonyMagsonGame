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
    public bool lockCursor;
    public bool gameplay;

    private bool _paused;
    private UI _ui;
   


    private void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.Escape) && !_paused)
        {
            Pause();
        }
        lockCursor = !_paused && gameplay ? true : false;

        if (_paused)
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
        _ui = PlayerLocatorSingleton.Instance.GetComponent<UI>();
    }
    public void InitializeGame(string levelName)
    {
       _currentScene = levelName;
        Lives = 3;
        _ui.updateLivesCount(Lives);
        CharacterManager.SpawnCharacter();
        LevelManager.UnloadScene("MainMenu");
        LevelManager.LoadLevelAdditively(levelName);
       
        gameplay = true;
        
    }


    public void RestartLevel()
    {

        

        _ui.updateLivesCount(Lives);
        LevelManager.UnloadScene(_currentScene);
        LevelManager.LoadLevelAdditively(_currentScene);
        Pause();

        WinMenu.SetActive(false);
        Losemenu.SetActive(false);

        Lives = 3;
        _ui.updateLivesCount(Lives);
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
            _paused = true;
        }
        else
        {
            Losemenu.SetActive(false);
            
            PlayerLocatorSingleton.Instance.GetComponent<PlayerController>().SnapBackToGround();
            _ui.updateLivesCount(Lives);
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

        _paused = true;
        Time.timeScale = 0;
    }

    public void Pause()
    {

        showCurrentTime();
        

        PauseMenu.SetActive(!_paused);

        Time.timeScale = _paused ? 1 : 0;
        _paused = !_paused;
    }

    private void showCurrentTime()
    {
        TimeText.text = _ui.gameTimer.ToString("F2");
        TimeText.gameObject.SetActive(!_paused);

        _ui.timerPaused = !_paused;
    }

}
