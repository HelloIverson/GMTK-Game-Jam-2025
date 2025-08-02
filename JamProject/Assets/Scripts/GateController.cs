using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    public bool activated;

    void Start()
    {
        
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
        transform.Find("gate_on_0").gameObject.SetActive(true);
        transform.Find("gate_off_0").gameObject.SetActive(false);
        transform.Find("Blocker").gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void deactivate()
    {
        activated = false;
        transform.Find("gate_on_0").gameObject.SetActive(false);
        transform.Find("gate_off_0").gameObject.SetActive(true);
        transform.Find("Blocker").gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
