using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MicrogameInputEvents
{
    [SerializeField] GameObject awayAnchor;
    [SerializeField] GameObject lowerTempAnchor;
    [SerializeField] GameObject increaseTempAnchor;
    [SerializeField] Animation buttonPressAnim;
    bool button1Held = false;
    bool button2Held = false;

    // Update is called once per frame
    void Update()
    {
        if (button1.IsPressed() && !button1Held)
        {
            transform.position = lowerTempAnchor.transform.position;
            buttonPressAnim.Play();
            button1Held = true;
        }
        else
        {
            transform.position = awayAnchor.transform.position;
            buttonPressAnim.Stop();
            button1Held = false;
        }

        if (button2.IsPressed() && !button2Held)
        {
            transform.position = lowerTempAnchor.transform.position;
            buttonPressAnim.Play();
            button2Held = true;
        }
        else
        {
            transform.position = awayAnchor.transform.position;
            buttonPressAnim.Stop();
            button2Held = false;
        }
    }
}
