using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Transform currentPlayer;
    public List<Transform> potentialPointsOfInterest;

    public float minSize = 2f;
    public float moveOffset = 0f; //for when following only the player
    public float zoomSpeed = 1f; // Speed of zooming in and out
    public float smoothSpeed = 5f; // Speed of camera movement smoothing
    public float smoothSpeedWhenZooming = 1f;
    public float bufferWhenZoomedOut = 1.05f;

    private Vector3 oldPlayer;
    private bool zoomingBackIn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        searchPotentialPOIs();

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
        for (int i = 0; i < potentialPointsOfInterest.Count; i++)
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

    void searchPotentialPOIs()
    {
        GameObject[] agentsPOIUpdate = GameObject.FindGameObjectsWithTag("Agent");
        GameObject[] guardsPOIUpdate = GameObject.FindGameObjectsWithTag("Guard");
        GameObject[] goalPOIUpdate = GameObject.FindGameObjectsWithTag("Goal");
        foreach (GameObject agent in agentsPOIUpdate)
        {
            potentialPointsOfInterest.Add(agent.transform);
        }
        foreach (GameObject guard in guardsPOIUpdate)
        {
            potentialPointsOfInterest.Add(guard.transform);
        }
        foreach (GameObject goal in goalPOIUpdate)
        {
            potentialPointsOfInterest.Add(goal.transform);
        }
    }
}
