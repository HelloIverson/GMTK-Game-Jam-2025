using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [SerializeField]
    private string nextScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextScene);
        if (nextScene != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
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
