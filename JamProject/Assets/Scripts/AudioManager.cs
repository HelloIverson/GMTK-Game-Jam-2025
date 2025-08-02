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
    private AudioSource currentSource;
    [SerializeField]
    private AudioSource nextSource;


    public float masterVolume = 0.75f; // Default master volume
    public float musicVolume = 1.0f; // Default music volume
    public float sfxVolume = 1.0f; // Default sound effects volume

    public float fadeTime = 1.0f; // Time to fade between songs
    private float fadeTimer = 0.0f;
    private bool isFading = false;
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
        // Your logic here
        Debug.Log("Scene changed");
        if (scene.name == "Start Menus")
        {
            if (currentSource.clip == null && nextSource.clip == null)
            {
                PlaySeamless(menusFirst, menusLoop);
            }
            else
            {
                isFading = true;
                fadeTimer = 0.0f;
                nextClipFirst = menusFirst;
                nextClipLoop = menusLoop;
            }

        }
        else if (scene.name == "Level 1") {
            isFading = true;
            fadeTimer = 0.0f;
            nextClipFirst = suspenseFirst;
            nextClipLoop = suspenseLoop;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
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
        currentSource.clip = currentClip;
        currentSource.PlayScheduled(startTime);

        // Schedule second clip
        nextSource.clip = nextClip;
        double nextStart = startTime + currentClip.length;
        nextSource.PlayScheduled(nextStart);

        // Optionally: swap sources for future chaining
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}
