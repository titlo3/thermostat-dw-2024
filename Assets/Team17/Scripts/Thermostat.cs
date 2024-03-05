using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thermostat : MicrogameInputEvents
{
    float temperature;
    float tempTarget;
    float tempVelocity = 0;
    public GameObject tempDial;
    public GameObject targetMarker;
    public SpriteRenderer indicator;
    bool button1Held = false;
    bool button2Held = false;
    [Header("Thermostat Range")]
    public float centerTemp = 20;
    public float tempRange = 30;
    [Header("Needle Properties")]
    [Range(0.001f, 0.02f)]
    public float tempAcceleration = 0.01f;
    [Range(0.2f, 4f)]
    public float sloppyness = 0.5f;
    [Range(0.05f, 0.5f)]
    public float acceptRange = 0.2f;

    void Start()
    {
        if (Random.Range(0f, 2f) > 1f) {
            tempTarget = Random.Range(0f, 5f) + (centerTemp + tempRange) - 5f;
        }
        else {
            tempTarget = Random.Range(0f, 5f) - (centerTemp - tempRange);
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

        if (Mathf.Abs(temperature - tempTarget) < acceptRange)
        {
            indicator.color = Color.green;
        }
        else { 
            indicator.color = Color.red;
        }

        if (tempVelocity < 0) tempVelocity += (tempAcceleration / sloppyness) * Time.deltaTime;
        else tempVelocity -= (tempAcceleration / sloppyness) * Time.deltaTime;

        if (Mathf.Abs(tempVelocity) < 0.0001f) tempVelocity = 0;
    }
}
