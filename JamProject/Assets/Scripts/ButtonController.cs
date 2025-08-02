using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool activated = false; //default state
    public bool isLever = false; //if button is a lever, then it will stay activated forever after being pressed
    public GateController gateController;

    //public GameObject[] buttons;
    private int agentsOnButton = 0;

    void Start()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform != null)
        {
            foreach (Transform testChild in parentTransform)
            {
                if (testChild.name == "Gate")
                {
                    gateController = testChild.GetComponent<GateController>();
                    break;
                }
            }
        }
        if (activated)
        {
            activate();
        } 
        else
        {
            deactivate();
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agent"))
        {
            agentsOnButton++;
            activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLever) //if this isnt a lever, then it will need to turn off when all agents step off
        {
            if (collision.CompareTag("Agent"))
            {
                agentsOnButton--;
                if (agentsOnButton <= 0) //could be == but as safety im checking <=
                {
                    deactivate();
                    agentsOnButton = 0;
                }
            }
        }
    }

    public void activate()
    {
        transform.Find("button_off_0").gameObject.SetActive(false);
        transform.Find("button_on_0").gameObject.SetActive(true);
        gateController.activate();

    }

    public void deactivate()
    {
        transform.Find("button_off_0").gameObject.SetActive(true);
        transform.Find("button_on_0").gameObject.SetActive(false);
        gateController.deactivate();

    }
}
