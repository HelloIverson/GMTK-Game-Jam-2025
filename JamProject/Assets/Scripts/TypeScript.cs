using UnityEngine;
using TMPro;

public class TypeScript : MonoBehaviour
{

    [SerializeField]
    private string[] messages;
    [SerializeField]
    private float textDelay = 0.01f; // Delay in milliseconds between each character
    [SerializeField]
    private float frontDelay;

    private TextMeshProUGUI text;
    private AudioSource audioSource;

    private string currentMessage;
    private int currentIndex = 0;
    private float timer;
    private int currentCharIndex = 0;
    private string currentPrint = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = -frontDelay;
        currentMessage = messages[0];
        text = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCharIndex < currentMessage.Length)
        {
            timer += Time.deltaTime;
        }
        if (timer > textDelay) {
            //print the next character of the current message
            currentPrint += currentMessage[currentCharIndex];
            if (currentMessage[currentCharIndex + 1] == ' ') {
                currentPrint += ' ';
                currentCharIndex++;
            }
            currentCharIndex++;
            text.text = currentPrint;
            timer = 0f;
            audioSource.Play();
        }
    }

    public void nextMessage() {
        currentIndex++;
        currentMessage = messages[currentIndex];
        currentPrint = "";

    }
}
