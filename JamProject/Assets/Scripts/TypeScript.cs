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
    [SerializeField]
    private bool isDialog;

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
        else if(isDialog){
            audioSource.loop = false;
        }
        if (timer > textDelay)
        {
            //print the next character of the current message
            currentPrint += currentMessage[currentCharIndex];
            if (currentMessage[currentCharIndex + 1] != null)
            {
                if (currentMessage[currentCharIndex + 1] == ' ') {
                    currentPrint += ' ';
                    currentCharIndex++;
                }
            }
            currentCharIndex++;
            text.text = currentPrint;
            timer = 0f;
            if (audioSource != null)
            {
                if (!isDialog || currentCharIndex == 1)
                {
                    audioSource.Play();
                }
                if (isDialog) {
                    if(currentCharIndex == 1) {
                        audioSource.loop = true;
                    }
                    audioSource.pitch += Random.Range(-0.02f, 0.02f);
                }
            }
        }
    }

    public void nextMessage() {
        if (!gameObject.activeSelf) {
            return;
        }
        // If the current message is not finished, skip to the end
        if (currentCharIndex < currentMessage.Length)
        {
            currentCharIndex = currentMessage.Length;
            currentPrint = currentMessage;
            text.text = currentPrint;
            return;
        }

        if (currentIndex >= messages.Length - 1)
        {
            return;
        }

        currentIndex++;
        currentCharIndex = 0;
        currentMessage = messages[currentIndex];
        currentPrint = "";

    }
}
