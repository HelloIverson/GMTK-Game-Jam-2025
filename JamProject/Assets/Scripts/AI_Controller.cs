using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public float speed;
    public UnityEngine.AI.NavMeshAgent agent;
    public float[] p1;
    public float[] p2;
    private int locate = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Atp2() == 1)
        {
            agent.SetDestination(new Vector3(p2[0], p2[1]));
        }
        if (Atp2() == 0)
        {
            agent.SetDestination(new Vector3(p1[0], p1[1]));
        }
        Debug.Log(Atp2());
        Debug.Log(locate);
    }
    public void GuardMovement()
    {
        agent.SetDestination(new Vector3(p1[0], p1[1]));
    }

    public void HearSound()
    {

    }

    public int Atp2()
    {
        if (agent.transform.position == new Vector3(p2[0], p2[1]))
        {
            locate = 0;
            return 0;
        }

        if (agent.transform.position == new Vector3(p1[0], p1[1]))
        {
            locate = 1;
            return 1;
        }
        else
        {
            return locate;
        }
    }

}
