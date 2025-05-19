using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;

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

    }
    private void InitializeGame()
    {
        levelManager.LoadLevelAdditively("SimpleLevel");
        characterManager.SpawnCharacter();
    }




}
