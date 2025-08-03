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
    private GameObject noise;

    public bool isWalking;
    public int buddies = 1;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        audioSource = GetComponent<AudioSource>();
        lastPos = transform.position;
        noise = transform.Find("Noise").gameObject;
        isWalking = false;
    }

    void Update()
    {
        if((lastPos - transform.position).magnitude > 0.001f)
        {
            isWalking = true;
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
            isWalking = false;
            stepTimer = 0.19f; // Reset timer if not moving
        }

        lastPos = transform.position;
        updateNoiseStrength();
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
                    if (newLayer == 3)
                    {
                        //new POI :)
                        transform.Find("Lantern").gameObject.SetActive(true);
                    }
                    else
                    {
                        //removing POI :(
                        if (!transform.CompareTag("Agent")) transform.Find("Lantern").gameObject.SetActive(false);
                    }
                    continue;
                }
                //Debug.Log("checking for guards");
                //For Yellow (can see guards), theres a special exception that checks for the distance away
                if (Vector3.Distance(obj.transform.position, transform.position) <= guardSightRange)
                {
                    obj.layer = newLayer;
                    if (newLayer == 3)
                    {
                        //new POI :)
                        transform.Find("Lantern").gameObject.SetActive(true);
                    }
                    else
                    {
                        //removing POI :(
                        if (!transform.CompareTag("Agent")) transform.Find("Lantern").gameObject.SetActive(false);
                    }
                    //Debug.Log("found guard");
                }
            }
        }
    }

    public void updateNoiseStrength()
    {
        float newNoiseStrength = (float)buddies * (isWalking ? 2 : 1); //double the noise strength if walking
        if (noise.GetComponent<NoiseScript>().noiseStrength > newNoiseStrength && isWalking)
        {
            //only decrease if !isWalking
            return;
        }
        noise.GetComponent<NoiseScript>().noiseStrength = newNoiseStrength;
    }

    public void updatePeopleInLoop(int peopleInLoop)
    {
        buddies = peopleInLoop;
    }
}
