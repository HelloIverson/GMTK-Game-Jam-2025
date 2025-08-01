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
            raycastForNewAgent();
        }
    }

    public void raycastForNewAgent()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 agentToMouse = (mousePos - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, agentToMouse, maxSwapDistance);

        if (hit.collider != null) //did it hit something?
        {
            if (hit.collider.CompareTag("Agent") && hit.collider.gameObject != selectedAgent) //was it another agent?
            {
                selectedAgent.transform.GetChild(2).gameObject.SetActive(false);
                changeSelectedAgent(hit.collider.gameObject);
                selectedAgent.transform.GetChild(2).gameObject.SetActive(true);
                Instantiate(particleSystem, selectedAgent.transform.position, Quaternion.identity);
            }
        } 
        else
        {
            Debug.Log("didn't find anything");
        }
    }

    public void changeSelectedAgent(GameObject newObject)
    {
        selectedAgent = newObject;
        camController.currentPlayer = newObject.transform;
        Debug.Log("Switched control to " + newObject.name);
    }
}
