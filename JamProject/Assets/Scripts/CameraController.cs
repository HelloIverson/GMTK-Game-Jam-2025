using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform currentPlayer;
    public Transform[] potentialPointsOfInterest;

    public float minSize;
    public float moveOffset; //for when following only the player
    public float zoomSpeed = 15f; // Speed of zooming in and out
    public float smoothSpeed = 0.125f; // Speed of camera movement smoothing

    private Vector3 oldPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        for(int i = 0; i < potentialPointsOfInterest.Length; i++)
        {
            //if potential point of interest has tag "PointOfInterest"
            if (potentialPointsOfInterest[i].CompareTag("PointOfInterest"))
            {
                Debug.Log("Found a point of interest: " + potentialPointsOfInterest[i].name);
                onlyPlayer = false;
                //log so the camera can see
            }
        }

        if (onlyPlayer)
        {
            Vector3 dv = currentPlayer.position - oldPlayer;
            Vector3 targetPos = new Vector3(currentPlayer.position.x + (dv.x * moveOffset), currentPlayer.position.y + (dv.y * moveOffset), transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
            //if size of camera is larger than minSize
            if (Camera.main.orthographicSize > minSize)
            {
                Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime * (0.01f + Camera.main.orthographicSize - minSize); // Zoom in
            }
            if(Camera.main.orthographicSize < minSize)
            {
                Camera.main.orthographicSize = minSize; // Clamp to minSize
            }

        }

        oldPlayer = currentPlayer.position;


    }
}
