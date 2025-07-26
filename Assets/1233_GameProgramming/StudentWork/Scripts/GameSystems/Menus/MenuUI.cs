using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuUI : MonoBehaviour
{


    public void pausedUI()
    {

       GameManager.Instance.Pause();
    }

    public void restartUI()
    {

        GameManager.Instance.RestartLevel();
    }

    public void mainMenu()
    {
        
        GameManager.Instance.Pause();
        GameManager.Instance.MainMenu();
    }
    public void nextStage()
    {
        Debug.Log("Enter Next Stage");
    }
   
}
