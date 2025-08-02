using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float guardSightRange = 10f;

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
        if((lastPos - transform.position).magnitude > 0.001)
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
    public void setPOIs(int newLayer, string tagsToUpdate) //changing is whether you are adding POIs (true) or removing them (false)
    {
        GameObject[] objectsToUpdate = GameObject.FindGameObjectsWithTag(tagsToUpdate);

        foreach (GameObject obj in objectsToUpdate)
        {
            if (obj.CompareTag(tagsToUpdate))
            {
                if (tagsToUpdate != "Guard")
                {
                    obj.layer = newLayer;
                    continue;
                }
                Debug.Log("checking for guards");
                //For Yellow (can see guards), theres a special exception that checks for the distance away
                if (Vector3.Distance(obj.transform.position, transform.position) <= guardSightRange)
                {
                    obj.layer = newLayer;
                    Debug.Log("found guard");
                }
            }
        }
    }
}
