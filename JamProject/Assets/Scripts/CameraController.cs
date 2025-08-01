using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public string defaultNameOfStartingPlayer;

    public Transform currentPlayer;
    public Transform[] potentialPointsOfInterest;

    public float minSize = 2f;
    public float moveOffset; //for when following only the player
    public float zoomSpeed = 15f; // Speed of zooming in and out
    public float smoothSpeed = 5f; // Speed of camera movement smoothing
    public float smoothSpeedWhenZooming = 10f;
    public float bufferWhenZoomedOut = 1.3f;

    private Vector3 oldPlayer;
    private bool zoomingBackIn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] tempAgents = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject testAgent in tempAgents)
        {
            if (testAgent.name == defaultNameOfStartingPlayer)
            {
                currentPlayer = testAgent.transform;
                break;
            }
        }

        //set the x and y of the camera to the current player
        if (currentPlayer != null)
        {
            transform.position = new Vector3(currentPlayer.position.x, currentPlayer.position.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Current player is not assigned in CameraController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool onlyPlayer = true;
        ArrayList POIX = new ArrayList();
        ArrayList POIY = new ArrayList();
        for (int i = 0; i < potentialPointsOfInterest.Length; i++)
        {
            //if potential point of interest is layer "Point of Interest"
            if (potentialPointsOfInterest[i].gameObject.layer == 3)
            {
                Debug.Log("Found a point of interest: " + potentialPointsOfInterest[i].name);
                onlyPlayer = false;
                POIX.Add(potentialPointsOfInterest[i].position.x);
                POIY.Add(potentialPointsOfInterest[i].position.y);
            }
        }

        if (onlyPlayer)
        {
            Vector3 dv = currentPlayer.position - oldPlayer;
            Vector3 targetPos = new Vector3(currentPlayer.position.x + (dv.x * moveOffset), currentPlayer.position.y + (dv.y * moveOffset), transform.position.z);
            if (zoomingBackIn)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeedWhenZooming);
            }
            else {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
            }
            //if size of camera is larger than minSize
            if (Camera.main.orthographicSize > minSize)
            {
                Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime * (0.01f + Camera.main.orthographicSize - minSize); // Zoom in
            }
            if (Camera.main.orthographicSize < minSize)
            {
                zoomingBackIn = false; // Reset zooming back in flag
                Camera.main.orthographicSize = minSize; // Clamp to minSize
            }

        }
        else {
            POIY.Add(currentPlayer.position.y);
            POIX.Add(currentPlayer.position.x);
            float topMost = (float) POIY[0];
            float bottomMost = (float) POIY[0];
            float leftMost = (float) POIX[0];
            float rightMost = (float) POIX[0];
            for (int i = 1; i < POIY.Count; i++) {
                if ((float) POIY[i] > topMost) {
                    topMost = (float) POIY[i];
                }
                if((float) POIY[i] < bottomMost) {
                    bottomMost = (float) POIY[i];
                }
                if((float) POIX[i] < leftMost) {
                    leftMost = (float) POIX[i];
                }
                if ((float) POIX[i] > rightMost) {
                    rightMost = (float) POIX[i];
                }
            }
            float midY = (topMost+ bottomMost) / 2f;
            float midX = (leftMost + rightMost) / 2f;
            Vector3 targetPos = new Vector3(midX, midY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeedWhenZooming);

            //now size
            float requiredHeightY = (topMost - bottomMost) * bufferWhenZoomedOut;
            float requiredHeightX = (rightMost - leftMost) * bufferWhenZoomedOut * (9/16);
            //set the camera size to the largest of requiredHeightY, requiredHeightX, and minSize
            float requiredSize = Mathf.Max(requiredHeightY, requiredHeightX, minSize);
            Camera.main.orthographicSize += (requiredSize - Camera.main.orthographicSize) * zoomSpeed * Time.deltaTime;
            zoomingBackIn = true;
        }

        oldPlayer = currentPlayer.position;


    }
}
