using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathText;
    public void Death()
    {

        deathText.text = "Game Over";
    }
}
