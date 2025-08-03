using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent2D : MonoBehaviour
{
    public UnityEvent[] onTriggerEnter;

    void OnTriggerEnter2D(Collider2D other)
    {
        //for each event
        foreach (var e in onTriggerEnter)
        {
            e.Invoke();
        }
    }
}