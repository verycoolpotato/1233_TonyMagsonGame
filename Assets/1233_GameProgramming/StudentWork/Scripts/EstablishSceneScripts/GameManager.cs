using StudentWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int lives;

    private void Awake()
    {
      
       if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
       instance = this;
        DontDestroyOnLoad(gameObject);

     
       
    }
    public void InitializeGame()
    {
        levelManager.UnloadScene("MainMenu");
        levelManager.LoadLevelAdditively("World");
        characterManager.SpawnCharacter();
        
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
        //play game over sequence

        levelManager.UnloadScene("World");
        levelManager.LoadLevelAdditively("MainMenu");
    }
    
}
