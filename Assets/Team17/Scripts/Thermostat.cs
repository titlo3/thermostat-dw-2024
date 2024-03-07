using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

namespace Team17
{
    public class Thermostat : MicrogameInputEvents
    {
        float temperature;
        float tempTarget;
        float tempVelocity = 0;
        public GameObject tempDial;
        public GameObject targetMarker;
        public TextMeshProUGUI winTimerText;

        [Header("Indicator")]
        public GameObject indicator;
        public Material indicatorRed;
        public Material indicatorGreen;

        float winTimer = 5;
        bool win = false;
        bool button1Held = false;
        bool button2Held = false;
        bool dadAdjusted = false;
        [Header("Thermostat Range")]
        public float centerTemp = 20;
        public float tempRange = 30;
        [Header("Needle Properties")]
        [Range(0.001f, 0.02f)]
        public float tempAcceleration = 0.01f;
        [Range(1f, 10f)]
        public float jumpScale = 5f;
        [Range(0.2f, 4f)]
        public float sloppyness = 0.5f;
        [Range(0.05f, 2f)]
        public float acceptRange = 0.2f;
        [Range(1, 5)]
        public float timeToWin = 3;
        [Range(1f, 10f)]
        public float dadStrength = 0;
        int dadAdjusting = -101;
        public Dad dad;

        [Header("Buttons")]
        public MeshRenderer buttonLeft;
        public MeshRenderer buttonRight;
        public Material buttonLight;
        public Material buttonDark;

        [Header("Countdown Bar")]
        public Slider countdown;

        float clock = 0;
        float tickClock = 0;


        protected override void OnGameStart()
        {
            base.OnGameStart();
            winTimer = timeToWin;
        }

        public float getHeat()
        {
            return (temperature + tempRange - centerTemp) / 2f * tempRange;
        }

        public float getTargetHeat()
        {
            return (tempTarget + tempRange - centerTemp) / 2f * tempRange;
        }

        public void setDadAdjust()
        {
            dadAdjusting = 45;
        }

        void Start()
        {
            if (Random.Range(0f, 2f) > 1f)
            {
                tempTarget = Random.Range(0f, 5f) + centerTemp + tempRange - 10f;
            }
            else
            {
                tempTarget = Random.Range(0f, 5f) + centerTemp - tempRange + 5f;
            }

            targetMarker.transform.eulerAngles = new Vector3(0, 0, -(((tempTarget - centerTemp) / tempRange) * 90f));

            temperature = centerTemp;

        }

        void Update()
        {
            if (button1.IsPressed() && !button1Held)
            {
                temperature -= (jumpScale / 10f) * Mathf.Abs(tempVelocity * 10f);
                tempVelocity -= tempAcceleration;
                button1Held = true;
            }
            else if (!button1.IsPressed())
            {
                button1Held = false;
            }
            if (button2.IsPressed() && !button2Held)
            {
                temperature += (jumpScale / 10f) * Mathf.Abs(tempVelocity * 10f);
                tempVelocity += tempAcceleration;
                button2Held = true;
            }
            else if (!button2.IsPressed())
            {
                button2Held = false;
            }
        }

        void FixedUpdate()
        {
            float velInverse = 1f - (Mathf.Abs(tempVelocity));
            float tickFreq = velInverse * velInverse * velInverse * velInverse * velInverse;
            tickClock += Time.deltaTime;
            if (tickClock > tickFreq && Mathf.Abs(tempVelocity) > 0.001f)
            {
                AudioHandler._instance.PlayThermostatTick();
                tickClock = 0;
            }


            clock += Time.deltaTime;

            if (clock % 1f > 0.5f)
            {
                if (temperature > tempTarget)
                {
                    buttonLeft.material = buttonLight;
                    buttonRight.material = buttonDark;
                }
                else
                {
                    buttonRight.material = buttonLight;
                    buttonLeft.material = buttonDark;
                }
            }
            else
            {
                buttonRight.material = buttonDark;
                buttonLeft.material = buttonDark;
            }

            /* DAD CHANGES*/

            if (dadStrength != 0)
            {
                if (dadAdjusting > -100)
                {
                    winTimerText.text = "Dad is adjusting";
                    dad.dadState = (int)Dad.dadStates.ADJUST;
                }

                if (dadAdjusting > 0)
                {
                    if (temperature < centerTemp)
                        tempVelocity += tempAcceleration / dadStrength;
                    else
                        tempVelocity -= tempAcceleration / dadStrength;
                }

                dadAdjusting--;
            }


            temperature += tempVelocity;
            if (temperature > centerTemp + tempRange)
            {
                temperature = centerTemp + tempRange;
                tempVelocity = 0;
            }
            if (temperature < centerTemp - tempRange)
            {
                temperature = centerTemp - tempRange;
                tempVelocity = 0;
            }
            tempDial.transform.eulerAngles = new Vector3(0, 0, -(((temperature - centerTemp) / tempRange) * 90f));

            if (Mathf.Abs(temperature - tempTarget) < acceptRange)
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorGreen;
                buttonLeft.material = buttonLight;
                buttonRight.material = buttonLight;
            }
            else
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorRed;
            }

            if (!win && Mathf.Abs(temperature - tempTarget) < acceptRange)
            {
                winTimer -= Time.deltaTime;
                winTimerText.text = winTimer.ToString("#.00");
                if (!dadAdjusted && winTimer <= 2)
                {
                    dadAdjusting = 45;
                    dadAdjusted = true;
                }
                if (winTimer <= 0)
                {
                    win = true;
                    ReportGameCompletedEarly();
                }
            }
            else if (!win && dadAdjusting < -100)
            {
                //winTimer = timeToWin;
                winTimerText.text = "";
            }

            if (win)
            {
                tempVelocity = 0;
                dadStrength = 100;
                jumpScale = 0;
                winTimerText.text = "You Win!";
            }


            if (tempVelocity < 0) tempVelocity += (tempAcceleration / sloppyness) * Time.deltaTime;
            else tempVelocity -= (tempAcceleration / sloppyness) * Time.deltaTime;

            if (Mathf.Abs(tempVelocity) < 0.0001f) tempVelocity = 0;
        }
    }
}