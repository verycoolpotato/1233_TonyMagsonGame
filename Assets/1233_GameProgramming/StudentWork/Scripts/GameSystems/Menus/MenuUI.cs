using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuUI : MonoBehaviour
{


    public void pausedUI()
    {

       GameManager.instance.Pause();
    }

    public void restartUI()
    {

        GameManager.instance.RestartLevel();
    }

    public void mainMenu()
    {
        GameManager.instance.Pause();
        GameManager.instance.MainMenu();
    }
    private void GameOverUI()
    {
      
    }

    private void GameWinUI()
    {

    }
}
