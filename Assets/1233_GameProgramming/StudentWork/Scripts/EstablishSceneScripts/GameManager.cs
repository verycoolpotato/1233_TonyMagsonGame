using StudentWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject Losemenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] public LevelManager levelManager;
    [SerializeField] private int lives;
    [SerializeField] private UI ui;

    public bool gameplay;
    private bool paused;
    public bool lockCursor;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        lockCursor = !paused && gameplay ? true : false;

        if (lives <= 0)
        {
            
        }
        else
        {
            
;        }

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

    }
    public void InitializeGame()
    {
        lives = 3;
        characterManager.SpawnCharacter();
        levelManager.UnloadScene("MainMenu");
        levelManager.LoadLevelAdditively("World");
        gameplay = true;

    }


    public void RestartLevel()
    {
      

        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("World");
        Pause();
        Losemenu.SetActive(false);
        lives = 3;
        
        StartCoroutine(enableTimer());
       
    }

    IEnumerator enableTimer()
    {
        gameplay = false;
      yield return new WaitForSecondsRealtime(0.01f);
        gameplay = true;
        
    }

    public void LoseLife()
    {
        if (lives <= 0)
        {
            Losemenu.SetActive(true);
            paused = true;
        }
        else
        {
            Losemenu.SetActive(false);
            lives--;
            PlayerLocatorSingleton.Instance.GetComponent<PlayerController>().SnapBackToGround();
        }
    }

    
    public void MainMenu()
    {
        Losemenu.SetActive(false);
        gameplay = false;
        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("MainMenu");
    }
    public void GameWinSequence()
    {

        gameplay = false;
       MainMenu();
    }

    public void Pause()
    {

        pauseMenu.SetActive(!paused);

        Time.timeScale = paused ? 1 : 0;
        paused = !paused;
    }

}
