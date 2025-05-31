using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace StudentWork 
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private Image crosshair;

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


