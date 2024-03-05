using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dad : MonoBehaviour
{
    public enum dadStates { 
        IDLE,
        ANTICIPATE,
        LOOK,
        ADJUST
    }

    int dadState;

    // Start is called before the first frame update
    void Start()
    {
        dadState = (int)dadStates.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
