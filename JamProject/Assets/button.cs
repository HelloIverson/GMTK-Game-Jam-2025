using UnityEngine;

public class button : MonoBehaviour
{private bool activated=false;
        private bool lever=false;
        public GameObject[] buttons;
        private GameObject selectedButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttons=GameObject.FindGameObjectsWithTag("Button");
        selectedButton = buttons?[0];
    }

    // Update is called once per frame
    void Update()
    {
      selectedButton.transform.GetChild(0).gameObject.SetActive(false);
      selectedButton.transform.GetChild(1).gameObject.SetActive(false);
    }
}
