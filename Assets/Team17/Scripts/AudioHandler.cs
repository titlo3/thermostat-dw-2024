using Unity.VisualScripting;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler _instance;

    //Audio Sources
    [Header("Audio Sources")]
    [SerializeField] AudioSource source_musicSource;
    [SerializeField] AudioSource source_thermostatSounds;
    [SerializeField] AudioSource source_dadSounds;
    [SerializeField] AudioSource source_sfx;

    //Create lists for audio groups
    [Header("Audio Groups")]
    [SerializeField] AudioClip[] dadFakeOuts;
    [SerializeField] AudioClip[] couchRuffles;
    [SerializeField] AudioClip[] dadYells;
    [SerializeField] AudioClip[] buttonClick;
    [SerializeField] AudioClip[] thermostatTick;
    [Header("Looping Audio")]

    //Music stuff
    [Header("Music Control")]
    [SerializeField][Tooltip("Determines how the audio fades back in")] AnimationCurve fadeInCurve;
    [SerializeField][Range(0, 5)] float fadeInTime;
    float fadeInTimeCurrent;
    bool fadingIn = false;

    private void Start()
    {
        //SET UP OBJECT AS SINGLETON
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("Audiohandler set successfully");
        }
        else
            Destroy(gameObject);

        fadeInTimeCurrent = fadeInTime;

        CutMusic();
        ReturnMusic();
    }


    //AUDIO EVENTS
    public void PlayFakeOut()
    {
        int randInt = Random.Range(0, dadFakeOuts.Length);

        for (int i = 0; i < dadFakeOuts.Length; i++)
        {
            if (i == randInt)
            {
                source_dadSounds.PlayOneShot(dadFakeOuts[i]);
            }
        }
    }

    public void PlayCouchRuffle()
    {
        int randInt = Random.Range(0, couchRuffles.Length);

        for (int i = 0; i < couchRuffles.Length; i++)
        {
            if (i == randInt)
            {
                source_sfx.PlayOneShot(couchRuffles[i]);
            }
        }
    }
    public void PlayYell()
    {
        int randInt = Random.Range(0, dadYells.Length);

        for (int i = 0; i < dadYells.Length; i++)
        {
            if (i == randInt)
            {
                source_dadSounds.PlayOneShot(dadYells[i]);
            }
        }
    }
    public void PlayButtonClick()
    {
        int randInt = Random.Range(0, buttonClick.Length);

        for (int i = 0; i < buttonClick.Length; i++)
        {
            if (i == randInt)
            {
                source_thermostatSounds.PlayOneShot(buttonClick[i]);
            }
        }
    }
    public void PlayThermostatTick()
    {
        int randInt = Random.Range(0, thermostatTick.Length);

        for (int i = 0; i < thermostatTick.Length; i++)
        {
            if (i == randInt)
            {
                source_thermostatSounds.PlayOneShot(thermostatTick[i]);
            }
        }
    }

    private void Update()
    {
        //HANDLE AUDIO FADES
        if (fadeInTimeCurrent < fadeInTime && fadingIn)
        {
            fadeInTimeCurrent += Time.deltaTime;

            source_musicSource.volume = fadeInCurve.Evaluate(fadeInTimeCurrent / fadeInTime);
        }

    }


    //FOR WHEN DAD LOOKS AT YOU
    public void CutMusic()
    {
        source_musicSource.volume = 0;
        fadeInTimeCurrent = 0;
        fadingIn = false;
    }

    public void ReturnMusic()
    {
        fadingIn = true;
    }
}
