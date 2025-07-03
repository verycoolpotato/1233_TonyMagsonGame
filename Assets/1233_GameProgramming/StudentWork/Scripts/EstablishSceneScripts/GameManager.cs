using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

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

      InitializeGame();
       
    }
    private void InitializeGame()
    {
        levelManager.LoadLevelAdditively("World");
        characterManager.SpawnCharacter();
    }
    
   

   
}
