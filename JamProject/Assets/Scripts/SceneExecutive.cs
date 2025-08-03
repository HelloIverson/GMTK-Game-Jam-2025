using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneExecutive : MonoBehaviour
{

    [SerializeField]
    private string nextScene;
    [SerializeField]
    private float fadeOutTime;
    [SerializeField]
    private float fadeInTime;
    [SerializeField]
    private Slider[] volumeSliders; // Sliders for master, music, and SFX volume

    public Image blackScreen;

    private bool fadeOut;
    private bool fadeIn;
    private string fadeOutScene;

    private GameObject audioManager;

    void Awake() {
        if (AudioManager.instance != null) {
            audioManager = AudioManager.instance.gameObject;
            Debug.Log("created audio manager");
        }
        Debug.Log(audioManager ? true : false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeOut = false;
        if (fadeInTime > 0f)
        {
            fadeIn = true;
            blackScreen.color = new Color(0f, 0f, 0f, 1f); // Start with black screen
            blackScreen.gameObject.SetActive(true); // Ensure black screen is active
        }
        else {
            fadeIn = false;
            blackScreen.color = new Color(0f, 0f, 0f, 0f); // No fade in, start transparent
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn) {
            Color color = blackScreen.color;
            color.a -= Time.deltaTime / fadeInTime;
            blackScreen.color = color;
            if (color.a <= 0f)
            {
                fadeIn = false;
                blackScreen.gameObject.SetActive(false); // Disable black screen after fade in
            }
        }

        if (fadeOut) {
            Color color = blackScreen.color;
            color.a += Time.deltaTime / fadeOutTime;
            blackScreen.color = color;
            if (color.a >= 1f)
            {
                GoToScene(fadeOutScene);
            }
        }
    }

    public void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextScene);
        if (nextScene != null)
        {
            LoadScene(nextScene);
        }
        else
        {
            Debug.LogWarning("Next scene is not assigned in SceneManager.");
        }
    }

    public void restart() {
        Debug.Log("Restarting current scene: " + SceneManager.GetActiveScene().name);
        GoToScene(SceneManager.GetActiveScene().name);
    }

    public void stopTime() {
        Time.timeScale = 0f;
    }

    public void resumeTime() {
        Time.timeScale = 1f;
    }

    public void LoadScene(string sceneName) {
        fadeOutScene = sceneName;
        fadeOut = true;
        blackScreen.gameObject.SetActive(true); // Ensure black screen is active during fade out
    }

    public void ToMenusMusic()
    {
        audioManager.GetComponent<AudioManager>().ToMenusMusic();
    }

    public void ToSuspenseMusic()
    {
        audioManager.GetComponent<AudioManager>().ToSuspenseMusic();
    }

    public void FadeToSuspenseMusic()
    {
        audioManager.GetComponent<AudioManager>().FadeToSuspenseMusic();
    }

    public void FadeToChaseMusic()
    {
        audioManager.GetComponent<AudioManager>().FadeToChaseMusic();
    }

    public void ChangeMasterVolume(float volume)
    {
        audioManager.GetComponent<AudioManager>().ChangeMasterVolume(volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        audioManager.GetComponent<AudioManager>().ChangeMusicVolume(volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        audioManager.GetComponent<AudioManager>().ChangeSFXVolume(volume);
    }

    public void SetVolumeSliders()
    {
        if (audioManager != null)
        {
            AudioManager audioManagerScript = audioManager.GetComponent<AudioManager>();
            volumeSliders[0].value = audioManagerScript.masterVolume;
            volumeSliders[1].value = audioManagerScript.sfxVolume;
            volumeSliders[2].value = audioManagerScript.musicVolume;
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }
    }

    private void GoToScene(string sceneName)
    {
        Debug.Log("Going to scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
