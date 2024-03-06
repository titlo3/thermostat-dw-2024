using UnityEngine;

public class Dad : MicrogameInputEvents
{
    [System.Serializable]
    public enum dadStates
    {
        IDLE,
        ANTICIPATE,
        LOOK,
        ADJUST
    }

    [SerializeField] int dadState;
    int previousDadState = -1;
    [SerializeField] SpriteRenderer sr;
    [Header("Sprites:")]
    [SerializeField] Sprite spriteIdle;
    [SerializeField] Sprite spriteAnticipate;
    [SerializeField] Sprite spriteLook;
    [SerializeField] Sprite spriteAdjust;

    [Header("Tuners:")]
    [SerializeField][Range(0f, 2f)][Tooltip("Determines how much invincibility time player has after dad turns around")] float iFrames;
    float iFrameCurrent;

    // Start is called before the first frame update
    void Start()
    {
        dadState = (int)dadStates.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the state has changed
        if (dadState != previousDadState)
        {
            // Fire a single function for the new state (audio, cues, etc...)
            switch ((dadStates)dadState)
            {
                case dadStates.IDLE:
                    sr.sprite = spriteIdle;
                    AudioHandler._instance.PlayCouchRuffle();
                    break;
                case dadStates.ANTICIPATE:
                    sr.sprite = spriteAnticipate;
                    AudioHandler._instance.PlayCouchRuffle();
                    AudioHandler._instance.PlayFakeOut();
                    break;
                case dadStates.LOOK:
                    sr.sprite = spriteLook;
                    AudioHandler._instance.PlayCouchRuffle();
                    AudioHandler._instance.PlayFakeOut();
                    break;
                case dadStates.ADJUST:
                    sr.sprite = spriteAdjust;
                    AudioHandler._instance.PlayCouchRuffle();
                    break;
            }
            // Update the previous state to the current state
            previousDadState = dadState;
        }

        //HANDLE PER FRAME STATE FUNCTIONS (CONSTANTLY UPDATED)
        switch ((dadStates)dadState)
        {
            case dadStates.IDLE:
                HandleIdle();
                break;
            case dadStates.ANTICIPATE:
                HandleAnticipate();
                break;
            case dadStates.LOOK:
                HandleLook();
                break;
            case dadStates.ADJUST:
                HandleAdjust();
                break;
        }

    }



    //UPDATE METHODS. EACH OF THESE METHODS WILL FIRE DURING THE APPROPRIATE STATES (EVERY FRAME)
    void HandleIdle()
    {

    }

    void HandleAnticipate()
    {

    }

    void HandleLook()
    {
        iFrameCurrent += Time.deltaTime;
        if (iFrameCurrent > iFrames && (button1.IsPressed() || button2.IsPressed()))
        {
            iFrameCurrent = 0;
            AudioHandler._instance.CutMusic();
            ReportGameCompletedEarly();
        }
    }

    void HandleAdjust()
    {

    }
}
