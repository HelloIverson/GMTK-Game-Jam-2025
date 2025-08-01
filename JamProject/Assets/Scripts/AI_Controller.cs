using System;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public float speed;
    public UnityEngine.AI.NavMeshAgent agent;
    public int guardX;
    public int guardY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(new Vector3(guardX, guardY));
    }

    public void guardMovement()
    {
        agent.SetDestination(new Vector3(guardX, guardY));
    }

    public void hearSound()
    {

    }


}
