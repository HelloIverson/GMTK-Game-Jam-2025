using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField]
    private AudioClip[] footstepSounds;
    [SerializeField]
    private float stepTime = 0.2f; // Time between footsteps

    private AudioSource audioSource;
    private float stepTimer = 0.19f; // Timer to track when to play the next step sound
    private Vector3 lastPos;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        audioSource = GetComponent<AudioSource>();
        lastPos = transform.position;
    }

    void Update()
    {
        if(lastPos != transform.position)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepTime)
            {
                AudioClip audioClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.PlayOneShot(audioClip);
                stepTimer -= stepTime; // Reset the timer after playing a sound (kind of)
            }
        }
        else
        {
            stepTimer = 0.19f; // Reset timer if not moving
        }
        lastPos = transform.position;
    }

    public void updateDestination()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        agent.SetDestination(mousePos);
    }
    
    [ContextMenu("test")]
    public void setPOIs() //changing is whether you are adding POIs (true) or removing them (false)
    {
        GameObject[] objectsToUpdate = GameObject.FindGameObjectsWithTag("Goal");

        foreach (GameObject obj in objectsToUpdate)
        {
            if (obj.CompareTag("Goal"))
            {
                //obj.layer = changingtrue ? 3 : 0;
            }
        }
    }
}
