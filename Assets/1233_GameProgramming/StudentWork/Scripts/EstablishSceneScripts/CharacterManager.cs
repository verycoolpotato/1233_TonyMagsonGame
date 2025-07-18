using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    private GameObject characterInstance;
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
        else
        {
            characterInstance.transform.position = spawnPosition;
        }
        
    }
    private void CharacterActive()
    {
        Cursor.lockState = GameManager.instance.gameplay ? CursorLockMode.Locked : CursorLockMode.None;
        characterInstance.SetActive(GameManager.instance.gameplay);
        
    }
   
}