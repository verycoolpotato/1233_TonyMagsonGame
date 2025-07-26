using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    public GameObject characterInstance;
    private void Update()
    {
        CharacterActive();
    }
    public void SpawnCharacter()
    {
        Vector3 spawnPosition = Vector3.zero;
        if (PlayerLocatorSingleton.Instance == null)
        {
            characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity, transform);
        }
        
          
        
        
    }
    private void CharacterActive()
    {
        Cursor.lockState = GameManager.Instance.lockCursor
            ? CursorLockMode.Locked : CursorLockMode.None;

        characterInstance.SetActive(GameManager.Instance.gameplay);
        
    }


   
}