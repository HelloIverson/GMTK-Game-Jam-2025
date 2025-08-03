using UnityEngine;
using UnityEngine.AI;

public class AI_Controller : MonoBehaviour
{
    public Transform[] waypoints; // array to hold your target points
    public NavMeshAgent guardNavMeshAgent;
    private int currentWaypointIndex = 0;
    public SceneExecutive sceneManager;
    private bool waitingForNextWaypoint = false;

    void Start()
    {
        guardNavMeshAgent.updateRotation = false;
        guardNavMeshAgent.updateUpAxis = false;
        guardNavMeshAgent = GetComponent<NavMeshAgent>();
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
            currentWaypointIndex++;
            currentWaypointIndex %= waypoints.Length;
            guardNavMeshAgent.destination = waypoints[currentWaypointIndex].position;
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
    }

    public void handleNoise(Transform source, float strength)
    {
        float panic = 0;
        if (panic < strength / 2)
        {
            panic = strength / 2;
        }
        guardNavMeshAgent.speed = panic;
        for (int i = 0; i < waypoints.Length + 1; i++)
        {
            waypoints[i] = source;
        }

    }

    public void chaseMusic()
    {
        //sceneManager.FadeToChaseMusic();
    }
    public void suspenseMusic()
    {
        //sceneManager.FadeToSuspenseMusic();
    }

}
