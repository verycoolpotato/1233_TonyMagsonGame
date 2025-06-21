using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;

    [Tooltip("Gameobjects to add to the level over time")]
    [SerializeField] private GameObject[] MapFeatures;

    private int featureNumber;

    private void Awake()
    {
       if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
       Instance = this;
        DontDestroyOnLoad(gameObject);

      InitializeGame();
        InvokeRepeating(nameof(AddToLevel), 5, 5);
    }
    private void InitializeGame()
    {
        levelManager.LoadLevelAdditively("World");
        characterManager.SpawnCharacter();
    }
    
    private void AddToLevel()
    {
        if (featureNumber < MapFeatures.Length)
        {
            Instantiate(MapFeatures[featureNumber], Vector3.zero, Quaternion.identity);
        }
       

        featureNumber++;
    }


}
