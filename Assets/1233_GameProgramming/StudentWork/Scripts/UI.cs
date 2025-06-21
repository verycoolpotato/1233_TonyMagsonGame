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

        private void Update()
        {
            

            CrosshairVisible();
        }
        private void CrosshairVisible()
        {
            crosshair.enabled = _input.Aim;
        }


        public void UpdateKnockbackNumber(float percentage)
        {
            knockbackPercentDisplay.text = percentage.ToString() + "%";
        }

        
    }

}


