using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        // stolen from https://www.youtube.com/watch?v=mJu-zdZ9dyE

        Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(movePosition, out var hitInfo))
        {
            agent.SetDestination(hitInfo.point);
        }
    }
}
