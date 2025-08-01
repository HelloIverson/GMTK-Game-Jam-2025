using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        
    }

    public void updateDestination()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        agent.SetDestination(mousePos);
        Debug.Log("moved");
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
