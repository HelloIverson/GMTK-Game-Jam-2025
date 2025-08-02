using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    [SerializeField]
    private AudioClip menusFirst;
    [SerializeField]
    private AudioClip menusLoop;
    [SerializeField]
    private AudioClip suspenseFirst;
    [SerializeField]
    private AudioClip suspenseLoop;
    [SerializeField]
    private AudioClip chaseFirst;
    [SerializeField]
    private AudioClip chaseLoop;
    [SerializeField]
    private AudioSource firstSource;
    [SerializeField]
    private AudioSource loopSource;
    [SerializeField]
    private AudioSource tempSource1;
    [SerializeField]
    private AudioSource tempSource2;


    public float masterVolume = 0.75f; // Default master volume
    public float musicVolume = 1.0f; // Default music volume
    public float sfxVolume = 1.0f; // Default sound effects volume

    public float fadeTime = 1.0f; // Time to fade between songs
    private float fadeTimer = 0.0f;
    private bool isFading = false;
    private bool isFadingIn = false;
    private AudioClip nextClipFirst;
    private AudioClip nextClipLoop;

    private void Awake()
    {
        // Ensure the object persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if they exist
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(masterVolume));
        audioMixer.SetFloat("MusicVolume", LinearToDecibel(musicVolume));
        audioMixer.SetFloat("SFXVolume", LinearToDecibel(sfxVolume));

        //loop through every AudioSource in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            //check if the audio source's gameobject is a child of this gameobject
            if (source.transform.parent != transform)
            {
                source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; // Set default group to SFX
            }
            
        }

        // Set the initial music based on the current scene
        PlaySeamless(menusFirst, menusLoop);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isFading)
        {
            fadeTimer += Time.deltaTime;
            if (fadeTimer >= fadeTime)
            {
                fadeTimer = 0f;
                isFading = false;
                if (isFadingIn)
                {
                    var temp = firstSource;
                    var temp2 = loopSource;
                    firstSource = tempSource1;
                    loopSource = tempSource2;
                    tempSource1 = temp;
                    tempSource2 = temp2;
                    tempSource2.Stop();
                    tempSource1.Stop();
                    isFadingIn = false;
                }
                else
                {
                    PlaySeamless(nextClipFirst, nextClipLoop);
                    firstSource.volume = 1;
                    loopSource.volume = 1;
                }
            }
            else
            {
                // Adjust volume of current source to fade out
                float volume = firstSource.volume - (Time.deltaTime / fadeTime);
                firstSource.volume = volume;
                loopSource.volume = volume;
                Debug.Log("Fading out: " + fadeTimer + " / " + fadeTime);
            }
            if (isFadingIn)
            {
                // Adjust volume of new source to fade in
                float volume = Mathf.Lerp(0.0f, 1.0f, fadeTimer / fadeTime);
                tempSource1.volume = volume * musicVolume;
                tempSource2.volume = volume * musicVolume;
            }
        }


    }

    public void ToMenusMusic() {
        isFading = true;
        nextClipFirst = menusFirst;
        nextClipLoop = menusLoop;
    }

    public void ToSuspenseMusic() {
        isFading = true;
        nextClipFirst = suspenseFirst;
        nextClipLoop = suspenseLoop;
    }

    public void FadeToSuspenseMusic() {
        tempSource2.volume = 0;
        tempSource1.volume = 0;
        isFading = true;
        isFadingIn = true;
        var temp = firstSource;
        var temp2 = loopSource;
        firstSource = tempSource1;
        loopSource = tempSource2;
        PlaySeamless(suspenseFirst, suspenseLoop);
        firstSource = temp;
        loopSource = temp2;
    }

    public void FadeToChaseMusic() {
        tempSource2.volume = 0;
        tempSource1.volume = 0;
        isFading = true;
        isFadingIn = true;
        var temp = firstSource;
        var temp2 = loopSource;
        firstSource = tempSource1;
        loopSource = tempSource2;
        PlaySeamless(chaseFirst, chaseLoop);
        firstSource = temp;
        loopSource = temp2;
    }

    public void ChangeMasterVolume(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(masterVolume));
    }

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("MusicVolume", LinearToDecibel(musicVolume));
    }

    public void ChangeSFXVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("SFXVolume", LinearToDecibel(sfxVolume));
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f; // Effectively muted
        return Mathf.Log10(linear) * 20f;
    }

    private void PlaySeamless(AudioClip currentClip, AudioClip nextClip)
    {
        double startTime = AudioSettings.dspTime + 0.1;

        // Play first clip
        firstSource.clip = currentClip;
        firstSource.PlayScheduled(startTime);

        // Schedule second clip
        loopSource.clip = nextClip;
        double nextStart = startTime + currentClip.length;
        loopSource.PlayScheduled(nextStart);
    }
}
