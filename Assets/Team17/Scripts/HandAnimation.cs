using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17
{
    public class HandAnimation : MicrogameInputEvents
    {
        [SerializeField] GameObject awayAnchor;
        [SerializeField] Animator buttonPressAnim;
        bool button1Held = false;
        bool button2Held = false;

        [Header("Buttons")]
        public MeshRenderer buttonLeft;
        public MeshRenderer buttonRight;
        public Material buttonLight;
        public Material buttonDark;

        public float lightTimer;
        public float lightTimerMax = 0.1f;

        // Update is called once per frame
        void Update()
        {
            
            if (lightTimer > 0)
            {
                lightTimer -= Time.deltaTime;
            } else
            {
                buttonLeft.material = buttonDark;
                buttonRight.material = buttonDark;
            }

            if (button1.IsPressed() && !button1Held)
            {
                buttonPressAnim.Play("Hand_LowButtonPress");
                button1Held = true;

                buttonLeft.material = buttonLight;
                lightTimer = lightTimerMax;

            }
            else if (!button1.IsPressed())
            {
                transform.position = awayAnchor.transform.position;
                buttonPressAnim.StopPlayback();
                button1Held = false;
            }

            if (button2.IsPressed() && !button2Held)
            {
                buttonPressAnim.Play("Hand_IncButtonPress");
                button2Held = true;

                buttonRight.material = buttonLight;
                lightTimer = lightTimerMax;
            }
            else if (!button2.IsPressed())
            {
                buttonPressAnim.StopPlayback();
                button2Held = false;
            }

            if (!button1Held && !button2Held)
            {
                transform.position = awayAnchor.transform.position;
            }
        }
    }
}