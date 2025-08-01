using UnityEngine;

public class button : MonoBehaviour
{
    public bool activated = false; //default state
    private bool lever = false; //if button is a lever, then it will stay activated forever after being pressed

    public GameObject[] buttons;
    private GameObject selectedButton;

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
        if (!lever) activated = false; //resets button if theres nothing on it
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Agent")) //only agents can press the button (the button parts wont push themselves now lol)
        {
            activated = true;
        }
    }

    public bool isPressed() {
        return activated;
    }
}
