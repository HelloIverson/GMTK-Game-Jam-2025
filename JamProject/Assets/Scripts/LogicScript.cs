using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public float maxSwapDistance = 4f; // the maximum range you can switch characters
    public GameObject blueAgent;
    public GameObject redAgent;
    public GameObject yellowAgent;

    private GameObject selectedAgent;

    void Start()
    {
        selectedAgent = blueAgent;
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
            if (hit.collider.CompareTag("Agent") && hit.collider.gameObject != gameObject) //another agent?
            {
                changeSelectedAgent(hit.collider.name);
            }
        }
    }

    //Beware, this function is case sensitive and needs the exact name of the GameObject!
    public void changeSelectedAgent(string newName)
    {
        switch (newName)
        {
            case "Blue Agent":
                selectedAgent = blueAgent;
                break;
            case "Red Agent":
                selectedAgent = redAgent;
                break;
            case "Yellow Agent":
                selectedAgent = yellowAgent;
                break;
            default:
                Debug.Log("Uh oh! Couldn't find agent called " + newName);
                break;
        }

        Debug.Log("Switched control to " + newName);
    }
}
