using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    private GameObject characterInstance;

    public void SpawnCharacter()
    {
        Vector3 spawnPosition = Vector3.zero;
        characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity, transform);
    }

    public void DestroyCharacter()
    {
        Destroy(characterInstance);
    }
}