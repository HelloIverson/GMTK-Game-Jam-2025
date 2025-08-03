using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public Transform[] waypoints; // Array to hold your target points
    public UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            // Set the initial destination to the first waypoint
        if (waypoints.Length > 0)
        {
                agent.destination = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        // Check if the agent has reached its current destination
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Cycle through waypoints
            agent.destination = waypoints[currentWaypointIndex].position;
            Debug.Log("updated");
        }
<<<<<<< Updated upstream
=======

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
>>>>>>> Stashed changes
    }

}
