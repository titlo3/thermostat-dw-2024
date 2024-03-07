using UnityEngine;

namespace Team17
{
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

        [SerializeField] public Thermostat thermostat;

        [SerializeField] public int dadState;
        int previousDadState = -1;
        [SerializeField] SpriteRenderer sr;
        [Header("Sprites:")]
        [SerializeField] Sprite spriteIdle;
        [SerializeField] Sprite spriteAnticipate;
        [SerializeField] Sprite spriteLook;
        [SerializeField] Sprite spriteAdjust;

        [Header("TV Sprites:")]
        [SerializeField] SpriteRenderer TV;
        [SerializeField] Sprite TVnormal;
        [SerializeField] Sprite TVadjust;

        [Header("Tuners:")]
        [SerializeField][Range(0f, 2f)][Tooltip("Determines how much invincibility time player has after dad turns around")] float iFrames;
        float iFrameCurrent;
        [SerializeField][Tooltip("Random Range in seconds for how long it takes before dad does a fakeout/look")] Vector2 randomStateSwitch;
        float timeTillNextSwitch;
        float timeTillNextSwitchCurrent;
        [SerializeField][Tooltip("Random Range in seconds before dad switches back to idle")] Vector2 randomIdleSwitch;
        float timeToSwitchToIdle;
        float timeToSwitchToIdleCurrent;

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
                        sr.color = Color.green; //TEMPORARY
                        TV.sprite = TVnormal;
                        AudioHandler._instance.ReturnMusic();
                        //AudioHandler._instance.PlayCouchRuffle();
                        timeTillNextSwitch = Random.Range(randomStateSwitch.x, randomStateSwitch.y); //Time until next random state switch
                        timeTillNextSwitchCurrent = 0;
                        break;
                    case dadStates.ANTICIPATE:
                        sr.sprite = spriteAnticipate;
                        sr.color = Color.yellow; //TEMPORARY
                        TV.sprite = TVnormal;
                        AudioHandler._instance.PlayCouchRuffle();
                        AudioHandler._instance.PlayFakeOut();
                        timeToSwitchToIdle = Random.Range(randomIdleSwitch.x, randomIdleSwitch.y); //Time until next random switch back to idle
                        timeToSwitchToIdleCurrent = 0;
                        iFrameCurrent = 0;
                        break;
                    case dadStates.LOOK:
                        sr.sprite = spriteLook;
                        sr.color = Color.red; //TEMPORARY
                        TV.sprite = TVnormal;
                        AudioHandler._instance.PlayCouchRuffle();
                        AudioHandler._instance.PlayFakeOut();
                        AudioHandler._instance.CutMusic();
                        timeToSwitchToIdle = Random.Range(randomIdleSwitch.x, randomIdleSwitch.y); //Time until next random switch back to idle
                        timeToSwitchToIdleCurrent = 0;
                        iFrameCurrent = 0;
                        break;
                    case dadStates.ADJUST:
                        sr.sprite = spriteAdjust;
                        sr.color = Color.green; //TEMPORARY
                        TV.sprite = TVadjust;
                        AudioHandler._instance.PlayCouchRuffle();
                        timeToSwitchToIdle = Random.Range(randomIdleSwitch.x, randomIdleSwitch.y); //Time until next random switch back to idle
                        timeToSwitchToIdleCurrent = 0;
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

        //Method that randomly switches to idle after random amount of time
        void SwitchToIdle()
        {
            //After time, switch back to idle
            if (timeToSwitchToIdleCurrent < timeToSwitchToIdle)
            {
                timeToSwitchToIdleCurrent += Time.deltaTime;
                //Debug.Log("WAITING TO SWITCH TO IDLE");
            }
            else if (timeToSwitchToIdleCurrent >= timeToSwitchToIdle)
            {
                int idleState = (int)dadStates.IDLE; //Randomly select a new state
                dadState = idleState;
                //Debug.Log("SWITCHING TO IDLE");
            }
        }

        //UPDATE METHODS. EACH OF THESE METHODS WILL FIRE DURING THE APPROPRIATE STATES (EVERY FRAME)
        void HandleIdle()
        {
            //RANDOMLY SELECT NEW STATE AFTER TIME
            if (timeTillNextSwitchCurrent < timeTillNextSwitch)
            {
                timeTillNextSwitchCurrent += Time.deltaTime;
                //Debug.Log("WAITING IN IDLE " + (timeTillNextSwitch - timeTillNextSwitchCurrent));
            }
            else if (timeTillNextSwitchCurrent >= timeTillNextSwitch)
            {
                int randomState = Random.Range(1, System.Enum.GetValues(typeof(dadStates)).Length - 1); //Randomly select a new state (except adjust)
                dadState = randomState;
                //Debug.Log("SWITCHING TO NEW RANDOM STATE");
            }
        }

        void HandleAnticipate()
        {
            //If player is caught by dad, go to look state
            iFrameCurrent += Time.deltaTime;
            if (iFrameCurrent > iFrames && (button1.IsPressed() || button2.IsPressed()))
            {
                //Debug.Log("PLAYER CAUGHT");
                dadState = (int)dadStates.LOOK;
            } //Else switch back to idle
            else if (iFrameCurrent > iFrames + 0.5f)
            {
                //Debug.Log("PLAYER NOT CAUGHT");
                SwitchToIdle();
            }
        }

        void HandleLook()
        {
            iFrameCurrent += Time.deltaTime;
            if (iFrameCurrent > iFrames && (button1.IsPressed() || button2.IsPressed()))
            {
                iFrameCurrent = 0;
                AudioHandler._instance.CutMusic();
                AudioHandler._instance.PlayYell();

                //ReportGameCompletedEarly();
                thermostat.setDadAdjust();

                //PLAY 3-second TV throw animation
            }
            else
            {
                //After time, switch back to idle
                if (timeToSwitchToIdleCurrent < 2.5f)
                {
                    timeToSwitchToIdleCurrent += Time.deltaTime;
                    //Debug.Log("WAITING TO SWITCH TO IDLE");
                }
                else if (timeToSwitchToIdleCurrent >= 2.5f)
                {
                    int idleState = (int)dadStates.IDLE; //Randomly select a new state
                    dadState = idleState;
                    //Debug.Log("SWITCHING TO IDLE");
                }
            }
        }

        void HandleAdjust()
        {
            SwitchToIdle();
        }
    }
}