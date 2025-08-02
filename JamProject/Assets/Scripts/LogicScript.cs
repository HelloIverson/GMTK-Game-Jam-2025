using UnityEngine;

public class LogicScript : MonoBehaviour
{
    //better raycasting
    // for some reason can only find the character if hovered?
    // make it an array to loop thru

    public string defaultNameOfStartingPlayer;

    public float maxSwapDistance = 4f; // the maximum range you can switch characters
    public GameObject[] agents;
    public CameraController camController;

    private GameObject selectedAgent;
    public GameObject particleSystem;
    private GameObject particles;

    void Start()
    {

        agents = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject testAgent in agents)
        {
            if (testAgent.name == defaultNameOfStartingPlayer)
            {
                changeSelectedAgent(testAgent);
                break;
            }
        }

        if (selectedAgent == null)
        {
            changeSelectedAgent(agents[0]);
            Debug.Log("Couldn't find agent named " + defaultNameOfStartingPlayer);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse button pressed
        {
            PlayerController selectedScript = selectedAgent.GetComponent<PlayerController>();
            selectedScript.updateDestination();
        }

        if (Input.GetKeyDown(KeyCode.Space)) // spacebar pressed
        {
            selectedAgent.transform.Find("Indicator").gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Space)) // spacebar released
        {
            selectedAgent.transform.Find("Indicator").gameObject.SetActive(false);
            GameObject newAgent = raycastForAgent();
            if (newAgent)
            {
                if (Input.GetKey(KeyCode.Z)) // z key held
                {

                }
                changeSelectedAgent(newAgent);
            }
        }
    }

    public GameObject raycastForAgent()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 agentToMouse = (mousePos - selectedAgent.transform.position).normalized;
        agentToMouse.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(selectedAgent.transform.position, agentToMouse, maxSwapDistance);

        if (hits.Length == 0)
        {
            Debug.Log("didn't find anything");
            return null;
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Agent") && hit.collider.gameObject != selectedAgent)
            {
                return hit.collider.gameObject;
            }
        }

        Debug.Log("no other agents found in raycast hits");
        return null;
    }

    public void changeSelectedAgent(GameObject newObject)
    {
            //in-loop settings (POI and highlight)
        if (selectedAgent) setLoopPrefs(selectedAgent, false); //turn off for old agent
        setLoopPrefs(newObject, true);                         //turn on for new agent

            //selectedagent
        selectedAgent = newObject;

            //camera
        camController.currentPlayer = newObject.transform;

            //particles
        particles = Instantiate(particleSystem, selectedAgent.transform.position, Quaternion.identity);
        Destroy(particles, 5f);

            //degbug
        Debug.Log("Switched control to " + newObject.name);
    }

    public void updatePointsOfInterest(bool setPOI, GameObject agent)
    {
        PlayerController selectedScript = agent.GetComponent<PlayerController>();
        int layer = setPOI ? 3 : 0; //if setting as a POI, put on layer 3, otherwise on layer 0
        string tagToCheck = "";
        switch(agent.name)
        {
            case "Red Agent":
                tagToCheck = "Goal";
                break;
            case "Blue Agent":
                tagToCheck = "Agent";
                break;
            case "Green Agent":
                tagToCheck = "Guard";
                break;
            default:
                //this agent has no POI-specific abilities
                break;
        }
        if (tagToCheck != "") selectedScript.setPOIs(layer, tagToCheck);
    }

    public void setLoopPrefs(GameObject agentToUpdate, bool toggleOn) //will toggle POIs and highlights
    {
        agentToUpdate.transform.GetChild(2).gameObject.SetActive(toggleOn);
        updatePointsOfInterest(toggleOn, agentToUpdate);
    }
}
