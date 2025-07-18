using StudentWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int lives;
    public bool gameplay;
    private bool paused;



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
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


    }

    public void LoseLife()
    {
        if (lives <= 0)
        {
            GameLoseSequence();
        }
        else
        {
            lives--;
            PlayerLocatorSingleton.Instance.GetComponent<PlayerController>().SnapBackToGround();
        }
    }

    private void GameLoseSequence()
    {
        gameOverScreen.Death();

        gameplay = false;
        levelManager.UnloadScene("World");
      
        levelManager.LoadLevelAdditively("MainMenu");
    }

    public void GameWinSequence()
    {

        gameplay = false;
        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("MainMenu");
    }

    public void Pause()
    {
       Time.timeScale = paused ? 1 : 0;

       paused = !paused;
    }

}
