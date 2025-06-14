using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    public void SpawnCharacter()
    {
        Vector3 spawnPosition = Vector3.zero;
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity,transform);
    }
  
}
