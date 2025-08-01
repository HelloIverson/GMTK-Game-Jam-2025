using UnityEngine;

public class button : MonoBehaviour
{
    public bool activated = false;
    //private bool lever = false;
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
        //activated = false; //resets button if theres nothing on it
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("something stayed!");
        if (collision.gameObject.CompareTag("Agent")) //only agents can press the button (the button parts wont push themselves now lol)
        {
            activated = true;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("something entered!");
    //}

    //void OnTr(Collider other) {
    //    activated = true;
    //}

    bool isPressed() {
        return activated;
    }
}
