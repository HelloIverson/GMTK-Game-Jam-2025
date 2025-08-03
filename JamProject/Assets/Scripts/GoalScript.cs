using UnityEngine;
using System.Collections.Generic;

public class AllInsideTriggerChecker : MonoBehaviour
{
    public List<GameObject> agents;
    public GameObject finishScreen;

    private BoxCollider2D boxCollider;
    private HashSet<GameObject> objectsInside = new HashSet<GameObject>();

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (agents.Contains(other.gameObject))
        {
            objectsInside.Add(other.gameObject);
            CheckAllInside();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (agents.Contains(other.gameObject))
        {
            objectsInside.Remove(other.gameObject);
        }
    }

    private void CheckAllInside()
    {
        if (objectsInside.Count == agents.Count)
        {
            finishScreen.SetActive(true);
        }
    }
}
