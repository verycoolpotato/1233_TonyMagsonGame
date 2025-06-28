using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace StudentWork 
{
    public class UI : MonoBehaviour
    {
        

        [Tooltip("Image that appears in the center of the screen while aiming")]
        [SerializeField] private Image crosshair;

        [Tooltip("Displays players current knockback percentage")]
        [SerializeField] private TextMeshProUGUI knockbackPercentDisplay;

        [Tooltip("Player input reference")]
        [SerializeField] private PlayerInputs _input;

        [Tooltip("GameTimer reference")]
        [SerializeField] private TextMeshProUGUI timerText;

        public float gameTimer {  get; private set; }

        private void GameTimer()
        {
            gameTimer += 1 * Time.deltaTime;
            timerText.text = gameTimer.ToString("F2");
        }

        private void Update()
        {

            GameTimer();
            CrosshairVisible();
        }
        //Show crosshair when player is aiming
        private void CrosshairVisible()
        {
            crosshair.enabled = _input.Aim;
        }

        //Called by player knockback controller
        public void UpdateKnockbackNumber(float percentage)
        {
            knockbackPercentDisplay.text = percentage.ToString() + "%";
            if (percentage > 70)
            {
                knockbackPercentDisplay.color = Color.red;
            }
        }

        
    }

}


