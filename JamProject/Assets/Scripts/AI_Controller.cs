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
    }

    public void handleNoise(Vector3 source, float strength)
    {

    }

}
