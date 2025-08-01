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

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, agentToMouse, maxSwapDistance);

        if (hits.Length == 0)
        {
            Debug.Log("didn't find anything");
            return;
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Agent") && hit.collider.gameObject != selectedAgent)
            {
                selectedAgent.transform.GetChild(2).gameObject.SetActive(false);
                changeSelectedAgent(hit.collider.gameObject);
                selectedAgent.transform.GetChild(2).gameObject.SetActive(true);
                Instantiate(particleSystem, selectedAgent.transform.position, Quaternion.identity);
                return; // stop after the first valid agent
            }
        }

        Debug.Log("no other agents found in raycast hits");
    }

    public void changeSelectedAgent(GameObject newObject)
    {
        selectedAgent = newObject;
        camController.currentPlayer = newObject.transform;
        Debug.Log("Switched control to " + newObject.name);
    }
}
