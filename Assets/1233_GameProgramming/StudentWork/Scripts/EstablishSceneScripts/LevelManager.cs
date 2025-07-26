using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   

    private void Awake()
    {
        LoadLevelAdditively("MainMenu");
    }
    public void LoadLevel(string levelName)
    {
        
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevelAdditively(string levelName)
    {
        SceneManager.LoadScene(levelName,LoadSceneMode.Additive);
    }
    public void UnloadScene(string levelName)
    {
        SceneManager.UnloadSceneAsync(levelName);
    }

   

}
