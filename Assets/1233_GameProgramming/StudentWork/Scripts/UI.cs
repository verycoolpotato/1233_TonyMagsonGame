using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace StudentWork 
{
    public class UI : MonoBehaviour
    {
        [Tooltip("Image that appears in the center of the screen while aiming")]
        [SerializeField] private Image crosshair;

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

    }

}


