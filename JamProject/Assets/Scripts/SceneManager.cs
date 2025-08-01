using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [SerializeField]
    private string nextScene;
    [SerializeField]
    private GameObject audio;
    [SerializeField]
    private float fadetime;
    [SerializeField]
    private GameObject blackScreen;

    private bool fadeOut;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut) {
            
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

    public void LoadScene(string sceneName) {
        Debug.Log("Loading scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
