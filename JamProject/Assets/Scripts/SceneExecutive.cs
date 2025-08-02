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
    
    public Image blackScreen;

    private bool fadeOut;
    private bool fadeIn;
    private string fadeOutScene;

    private GameObject audioManager;

    void Awake() {
        if (AudioManager.instance != null) {
            audioManager = AudioManager.instance.gameObject;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeOut = false;
        if (fadeInTime > 0f)
        {
            fadeIn = true;
            blackScreen.color = new Color(0f, 0f, 0f, 1f); // Start with black screen
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
            fadeOutScene = nextScene;
            fadeOut = true;
            blackScreen.gameObject.SetActive(true); // Ensure black screen is active during fade out
        }
        else
        {
            Debug.LogWarning("Next scene is not assigned in SceneManager.");
        }
    }

    public void LoadScene(string sceneName) {
        fadeOutScene = sceneName;
        fadeOut = true;
    }

    private void GoToScene(string sceneName)
    {
        Debug.Log("Going to scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
