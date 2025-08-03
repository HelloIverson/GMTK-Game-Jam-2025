using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AI_Controller : MonoBehaviour
{
    public Transform[] waypoints; // array to hold your target points
    public Transform[] monitorPoints; //saves old points for when panic dies down
    public NavMeshAgent guardNavMeshAgent;
    private int currentWaypointIndex = 0;
    public SceneExecutive sceneManager;
    public bool chasing = false;
    private bool waitingForNextWaypoint = false;
    private bool panicking = false;
    private float panic = 0;
    private Coroutine myCoroutineInstance;

    void Start()
    {
        guardNavMeshAgent.updateRotation = false;
        guardNavMeshAgent.updateUpAxis = false;
        guardNavMeshAgent = GetComponent<NavMeshAgent>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneExecutive>();
        // set the initial destination to the first waypoint
        if (waypoints.Length > 0)
        {
            guardNavMeshAgent.destination = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        // check if the agent has reached its current destination
        if (!guardNavMeshAgent.pathPending &&
            guardNavMeshAgent.remainingDistance <= guardNavMeshAgent.stoppingDistance &&
            !waitingForNextWaypoint)
        {
            waitingForNextWaypoint = true;
            // cycle through waypoints
            if (waypoints.Length > 0)
            {
                currentWaypointIndex++;
                currentWaypointIndex %= waypoints.Length;
                guardNavMeshAgent.destination = waypoints[currentWaypointIndex].position;
            }
            //Debug.Log(gameObject.name + ": moving to waypoint " + currentWaypointIndex);
        }

        if (guardNavMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            waitingForNextWaypoint = false;
        }

        if (guardNavMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            waitingForNextWaypoint = false;
        }
        if (!panicking)
        {
            for (int i = 0; i < monitorPoints.Length; i++)
            {
                waypoints[i] = monitorPoints[i];
                chasing = false;
            }
        }
    }

    public void handleNoise(Transform source, float strength)
    {
        // update guard waypoints based on noise
        if (panic < strength)
        {
            panic = strength;
        }

        guardNavMeshAgent.speed = panic / 4;

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = source;
            panicking = true;
            chasing = true;
        }
        // StopCoroutine(myCoroutineInstance);
        myCoroutineInstance = StartCoroutine(PanickingDelay());

        // new WaitForSeconds(panic);
        // panicking = false;
        // Debug.Log(panicking);


    }

    public void chaseMusic()
    {
        sceneManager.FadeToChaseMusic();
    }
    public void suspenseMusic()
    {
        sceneManager.FadeToSuspenseMusic();
    }

    IEnumerator PanickingDelay()
    {
        yield return new WaitForSeconds(panic);
        panicking = false;
        Debug.Log(panicking);
    }

}
