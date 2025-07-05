using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;

    

    
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

    
}
