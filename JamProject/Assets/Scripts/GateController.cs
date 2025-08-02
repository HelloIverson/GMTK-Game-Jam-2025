using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    public bool activated;
    public bool isHorizontal;

    void Start()
    {
       
             transform.Find("gate_horizontal_on_0").gameObject.SetActive(false);
        transform.Find("gate_horizontal_off_0").gameObject.SetActive(false); 
        transform.Find("gate_color_horizontal_0").gameObject.SetActive(false);
       
            transform.Find("gate_vertical_on_0").gameObject.SetActive(false);
        transform.Find("gate_vertical_off_0").gameObject.SetActive(false); 
        transform.Find("gate_color_vertical_0").gameObject.SetActive(false); 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Agent")) //button not activated
        {
            Debug.Log(collision.name + " bumped into a gate.");
            collision.gameObject.GetComponent<NavMeshAgent>().ResetPath();
        }
    }

    public void activate()
    {
        activated = true;
        if(isHorizontal) {
        transform.Find("gate_horizontal_on_0").gameObject.SetActive(true);
        transform.Find("gate_horizontal_off_0").gameObject.SetActive(false); 
        transform.Find("gate_color_horizontal_0").gameObject.SetActive(false); 
        } else {
        transform.Find("gate_vertical_on_0").gameObject.SetActive(true);
        transform.Find("gate_vertical_off_0").gameObject.SetActive(false); 
        transform.Find("gate_color_vertical_0").gameObject.SetActive(false); 
        }

        transform.Find("Blocker").gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void deactivate()
    {
        activated = false;
        if(isHorizontal) {
        transform.Find("gate_horizontal_on_0").gameObject.SetActive(false);
        transform.Find("gate_horizontal_off_0").gameObject.SetActive(true); 
        transform.Find("gate_color_horizontal_0").gameObject.SetActive(true); 
        } else {
        transform.Find("gate_vertical_on_0").gameObject.SetActive(false);
        transform.Find("gate_vertical_off_0").gameObject.SetActive(true); 
        transform.Find("gate_color_vertical_0").gameObject.SetActive(true); 
        }
        transform.Find("Blocker").gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
