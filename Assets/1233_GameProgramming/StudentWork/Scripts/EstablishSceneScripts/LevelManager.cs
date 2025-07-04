using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string LevelName)
    {
        
        SceneManager.LoadScene(LevelName);
    }

    public void LoadLevelAdditively(string LevelName)
    {
        SceneManager.LoadScene(LevelName,LoadSceneMode.Additive);
    }
    public void UnloadScene(string LevelName)
    {
        SceneManager.UnloadSceneAsync(LevelName);
    }
}
