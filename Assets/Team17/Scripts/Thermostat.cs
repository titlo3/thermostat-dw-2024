using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Thermostat : MicrogameInputEvents
{
    float temperature;
    float tempTarget;
    float tempVelocity = 0;
    public GameObject tempDial;
    public GameObject targetMarker;
    public SpriteRenderer indicator;
    public TextMeshProUGUI winTimerText;
    float winTimer = 5;
    bool win = false;
    bool button1Held = false;
    bool button2Held = false;
    bool windBlowingUp = true;
    [Header("Thermostat Range")]
    public float centerTemp = 20;
    public float tempRange = 30;
    [Header("Needle Properties")]
    [Range(0.001f, 0.02f)]
    public float tempAcceleration = 0.01f;
    [Range(0.2f, 4f)]
    public float sloppyness = 0.5f;
    [Range(0.05f, 2f)]
    public float acceptRange = 0.2f;
    [Range(1f, 30f)]
    public float pushForce = 20f;
    float windStrength = 0;
    float windDirectionTimer = 0;
    int dadAdjusting = 500;
    protected override void OnGameStart()
    {
        base.OnGameStart();
        windStrength = 20f;
    }

    void Start()
    {
        if (Random.Range(0f, 2f) > 1f) {
            tempTarget = Random.Range(0f, 5f) + (centerTemp + tempRange) - 7f;
        }
        else {
            tempTarget = Random.Range(2f, 7f) - (centerTemp - tempRange);
        }

        Debug.Log(tempTarget);

        targetMarker.transform.eulerAngles = new Vector3(0, 0, -(((tempTarget - centerTemp) / tempRange) * 90f));


        temperature = Random.Range(0f, 10f) + 15f;
    }

    void Update()
    {
        if (button1.IsPressed() && !button1Held) {
            tempVelocity -= tempAcceleration;
            button1Held = true;
        }
        else if(!button1.IsPressed()) {
            button1Held = false;
        }
        if (button2.IsPressed() && !button2Held) {
            tempVelocity += tempAcceleration;
            button2Held = true;
        }
        else if (!button2.IsPressed()){ 
            button2Held= false;
        }
    }

    void FixedUpdate()
    {
        /* RANDOM MOVEMENT
         
        if (windStrength != 0)
        {
            windStrength = 3;
            if (windBlowingUp)
            {
                tempVelocity += tempAcceleration / windStrength;
            }
            else
            {
                tempVelocity -= tempAcceleration / windStrength;
            }

            windDirectionTimer--;
            if (windDirectionTimer < 0)
            {
                windDirectionTimer = Random.Range(5, 10);
                windBlowingUp = !windBlowingUp;
            }
        }

        */

        /* DAD CHANGES
        */

        windStrength = 4;

        if (windStrength != 0)
        {
            if (dadAdjusting < 50) {
                winTimerText.text = "Dad is adjusting";
                if(temperature < centerTemp)
                    tempVelocity += tempAcceleration / windStrength;
                else
                    tempVelocity -= tempAcceleration / windStrength;
            }

            dadAdjusting--;
            if (dadAdjusting < 0)
            {
                dadAdjusting = Random.Range(500, 700);
            }
        }

        /*
        */

        /* CONSTANT PUSH
        

        if (windStrength != 0)
        {
            if (windBlowingUp)
            {
                tempVelocity += tempAcceleration / windStrength;
            }
            else
            {
                tempVelocity -= tempAcceleration / windStrength;
            }

            windDirectionTimer--;
            if (windDirectionTimer < 0)
            {
                windDirectionTimer = Random.Range(50, 100);
                windBlowingUp = !windBlowingUp;
            }
        }

        
        */

        temperature += tempVelocity;
        if (temperature > centerTemp + tempRange) {
            temperature = centerTemp + tempRange;
            tempVelocity = 0;
        }
        if (temperature < centerTemp - tempRange)
        {
            temperature = centerTemp - tempRange;
            tempVelocity = 0;
        }
        tempDial.transform.eulerAngles = new Vector3(0, 0, -(((temperature - centerTemp) / tempRange) * 90f));

        if (!win && Mathf.Abs(temperature - tempTarget) < acceptRange)
        {
            indicator.color = Color.green;
            winTimer -= Time.deltaTime;
            winTimerText.text = winTimer.ToString("#.00");
            if (winTimer <= 0) {
                win = true;
            }
        }
        else if(!win && dadAdjusting > 50) { 
            indicator.color = Color.red;
            winTimer = 3;
            winTimerText.text = "";
        }

        if (win) {
            tempVelocity = 0;
            windStrength = 0;
            winTimerText.text = "You Win!";
        }


        if (tempVelocity < 0) tempVelocity += (tempAcceleration / sloppyness) * Time.deltaTime;
        else tempVelocity -= (tempAcceleration / sloppyness) * Time.deltaTime;

        if (Mathf.Abs(tempVelocity) < 0.0001f) tempVelocity = 0;
    }
}
