using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public Transform[] waypoints; // Array to hold your target points
    public UnityEngine.AI.NavMeshAgent guardNavMeshAgent;
    private int currentWaypointIndex = 0;
    public SceneExecutive sceneManager;

    void Start()
    {
        guardNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // set the initial destination to the first waypoint
        if (waypoints.Length > 0)
        {
            guardNavMeshAgent.destination = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        // Check if the agent has reached its current destination
        if (!guardNavMeshAgent.pathPending && guardNavMeshAgent.remainingDistance < guardNavMeshAgent.stoppingDistance)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Cycle through waypoints
            guardNavMeshAgent.destination = waypoints[currentWaypointIndex].position;
            Debug.Log("updated");
        }
    }

    public void handleNoise(Vector3 source, float strength)
    {

    }

    public void giveChase()
    {
        //sceneManager.FadeToChaseMusic();
    }

}
