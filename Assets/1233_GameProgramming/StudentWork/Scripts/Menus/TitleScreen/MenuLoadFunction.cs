using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadFunction : MonoBehaviour
{
    public void StartGame()
    {
        Invoke(nameof(timer), 3);
    }
    private void timer()
    {
        if (GameManager.instance != null)
            GameManager.instance.InitializeGame();
    }
}
