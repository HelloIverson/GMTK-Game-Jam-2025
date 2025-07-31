using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public PlayerController blueAgentScript;
    public PlayerController redAgentScript;

    private string agentSelected;

    void Start()
    {
        //i should set blueAgent and redAgent w code but i lowk forgot how
        agentSelected = "blue";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left mouse button pressed
        {
            //this code is terrible but perfect is the enemy of good
            switch(agentSelected)
            {
                case "blue":
                    blueAgentScript.updateDestination();
                    break;
                case "red":
                    redAgentScript.updateDestination();
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ik this code is bad but its just placeholder so dw abt it
            if (agentSelected == "blue") changeSelectedAgent("red");
            else if (agentSelected == "red") changeSelectedAgent("blue");
        }
    }

    public void changeSelectedAgent(string newName)
    {
        agentSelected = newName;
        Debug.Log("Switched control to " + newName);
    }
}
