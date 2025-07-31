using System;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent <Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.forward * UnityEngine.Random.Range(-20, 20);
    }
}
