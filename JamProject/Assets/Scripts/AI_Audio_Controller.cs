using UnityEngine;

public class AI_Audio_Controller : MonoBehaviour
{

    public LogicScript logicScript;
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    public bool doesWalk = true; // Whether the AI can walk and play footsteps
    public float stepTime = 0.5f; // Time interval between footsteps
    public float fadeStrength = 1.0f;
    private float stepTimer = 0f; // Timer to track when to play the next step sound

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepTimer = 0.19f; // Initialize the step timer
    }

    // Update is called once per frame
    void Update()
    {
        if (doesWalk)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepTime)
            {
                //calculate volume
                float distanceToPlayer = Vector3.Distance(transform.position, logicScript.selectedAgent.transform.position);
                float volume = Mathf.Clamp01(1.0f - ((distanceToPlayer / fadeStrength)*(distanceToPlayer / fadeStrength)));
                audioSource.volume = volume;

                AudioClip audioClip = footsteps[Random.Range(0, footsteps.Length)];
                audioSource.PlayOneShot(audioClip);
                stepTimer -= stepTime; // Reset the timer after playing a sound (kind of)
            }
        }
    }
}
