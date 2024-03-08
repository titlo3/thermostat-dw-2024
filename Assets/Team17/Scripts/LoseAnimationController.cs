using UnityEngine;

public class LoseAnimationController : MonoBehaviour
{
    [System.Serializable]
    public enum States
    {
        DEFAULT,
        PICKUP,
        THROW
    }
    [SerializeField] States states = States.DEFAULT;
    [SerializeField] GameObject dadDefault;
    [SerializeField] GameObject dadPickUp;
    [SerializeField] GameObject dadThrow;

    [Header("TUNERS")]
    [SerializeField]
    [Range(0f,2f)] float timeToSwitch;
    float timeToSwitchC;

    [Header("=====# TV THROW STUFF #=====")]
    [SerializeField]
    [Range(0,3)] float timeToThrow;
    float timeToThrowC;
    [SerializeField] GameObject thrownTV;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case States.DEFAULT:
                NextState(States.PICKUP);
                dadDefault.SetActive(true);
                dadPickUp.SetActive(false);
                dadThrow.SetActive(false);
                break;
            case States.PICKUP:
                NextState(States.THROW);
                dadDefault.SetActive(false);
                dadPickUp.SetActive(true);
                dadThrow.SetActive(false);
                break;
            case States.THROW:
                dadDefault.SetActive(false);
                dadPickUp.SetActive(false);

                if (timeToThrowC > timeToThrow)
                {
                    timeToThrowC += Time.deltaTime;
                }
                else
                {
                    thrownTV.SetActive(true);
                }
                break;
        }
    }

    void NextState(States state)
    {
        if (timeToSwitchC < timeToSwitch)
        {
            timeToSwitchC += Time.deltaTime;
        }
        else
        {
            states = state;
            timeToSwitchC = 0f;
        }
    }
}
