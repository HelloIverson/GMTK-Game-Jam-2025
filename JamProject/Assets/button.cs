using UnityEngine;

public class button : MonoBehaviour
{
    public bool activated = false; //default state
    private bool lever = false; //if button is a lever, then it will stay activated forever after being pressed

    public GameObject[] buttons;
    private GameObject selectedButton;
    private int agentsOnButton = 0;

    void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        selectedButton = buttons?[0];
    }

    void Update()
    {
        //selectedButton.SetActive(true);
        selectedButton.transform.Find("button_off_0")?.gameObject.SetActive(!activated);
        selectedButton.transform.Find("button_on_0")?.gameObject.SetActive(activated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agent"))
        {
            agentsOnButton++;
            activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!lever) //if this isnt a lever, then it will need to turn off when all agents step off
        {
            if (collision.CompareTag("Agent"))
            {
                agentsOnButton--;
                if (agentsOnButton <= 0) //could be == but as safety im checking <=
                {
                    activated = false;
                    agentsOnButton = 0;
                }
            }
        }
    }

    public bool isPressed() {
        return activated;
    }
}
