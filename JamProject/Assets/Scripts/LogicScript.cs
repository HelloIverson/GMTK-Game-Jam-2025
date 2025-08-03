using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicScript : MonoBehaviour
{
    //better raycasting
    // for some reason can only find the character if hovered?
    // make it an array to loop thru

    [SerializeField]
    private AudioClip swapSound;
    [SerializeField]
    private AudioClip groupSound;
    [SerializeField]
    private AudioClip ungroupSound;
    [SerializeField]
    private bool isTutorial;
    [SerializeField]
    private GameObject[] dialogsForTutorials;

    public string defaultNameOfStartingPlayer;

    public float maxSwapDistance = 4f; // the maximum range you can switch characters
    public GameObject[] agents;
    public GameObject[] guards;
    public int chaseCount = 0;
    public CameraController camController;

    public GameObject selectedAgent;
    private GameObject particles;
    private List<GameObject> loopers = new(); // all the gameObjects of agents in the loop
    private AudioSource audioSource;

    public GameObject particleSystem;
    public bool inUI = false;
    public bool chaseMusic = false;
    public bool oldChaseMusic = true;

    void Start()
    {

        agents = GameObject.FindGameObjectsWithTag("Agent");
        guards = GameObject.FindGameObjectsWithTag("Guard");
        bool foundAgent = false;
        foreach (GameObject testAgent in agents)
        {
            if (testAgent.name == defaultNameOfStartingPlayer)
            {
                changeSelectedAgent(testAgent);
                setLoopPrefs(testAgent, true);
                loopers.Add(testAgent);
                foundAgent = true;
                break;
            }
        }
        if (!foundAgent)
        {
            Debug.Log("Couldn't find " + defaultNameOfStartingPlayer + ". Using first agent found, named " + agents[0]);
            changeSelectedAgent(agents[0]);
            setLoopPrefs(agents[0], true);
            loopers.Add(agents[0]);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!inUI)
        {
            // movement
            if (Input.GetMouseButtonUp(0)) // left mouse button pressed
            {
                foreach (GameObject agentInLoop in loopers)
                {
                    PlayerController selectedScript = agentInLoop.GetComponent<PlayerController>();
                    selectedScript.updateDestination();
                }
                if (isTutorial && dialogsForTutorials[1].activeSelf)
                {
                    dialogsForTutorials[1].SetActive(false);
                    dialogsForTutorials[2].SetActive(true);
                }
            }

            // indicator on
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)) // z key or x key pressed
            {
                selectedAgent.transform.Find("Indicator").gameObject.SetActive(true);
            }

            // talk
            if (Input.GetKeyUp(KeyCode.X) && !Input.GetKey(KeyCode.Z)) // x key released (and z not already pressed)
            {
                selectedAgent.transform.Find("Indicator").gameObject.SetActive(false);
                GameObject raycastedAgent = raycastForAgent();
                if (raycastedAgent)
                {
                    if (outsideLoopCheck(raycastedAgent))
                    {
                        loopers.Add(raycastedAgent);
                        setLoopPrefs(raycastedAgent, true);
                        audioSource.PlayOneShot(groupSound);
                        if (isTutorial && dialogsForTutorials[2].activeSelf)
                        {
                            dialogsForTutorials[2].SetActive(false);
                            dialogsForTutorials[3].SetActive(true);
                        }
                        foreach (GameObject agentInLoop in loopers)
                        {
                            agentInLoop.GetComponent<PlayerController>().updatePeopleInLoop(loopers.Count);
                        }
                    }
                }
            }

            // switch
            if (Input.GetKeyUp(KeyCode.Z) && !Input.GetKey(KeyCode.X)) // z key released (and x not already pressed)
            {
                selectedAgent.transform.Find("Indicator").gameObject.SetActive(false);
                GameObject raycastedAgent = raycastForAgent();
                if (raycastedAgent)
                {
                    if (outsideLoopCheck(raycastedAgent))
                    {
                        //we're switching outside the loop, so we need to clear whats left there
                        foreach (GameObject agentInLoop in loopers)
                        {
                            setLoopPrefs(agentInLoop, false);
                            agentInLoop.GetComponent<PlayerController>().updatePeopleInLoop(1);
                        }
                        loopers = new List<GameObject>();

                        //and now actually switch selectedAgent
                        loopers.Add(raycastedAgent);
                        setLoopPrefs(raycastedAgent, true);
                    }
                    changeSelectedAgent(raycastedAgent);
                    audioSource.PlayOneShot(swapSound);
                    if (isTutorial && dialogsForTutorials[0].activeSelf)
                    {
                        dialogsForTutorials[0].SetActive(false);
                        dialogsForTutorials[1].SetActive(true);
                    }
                }
            }

            // clear
            if (Input.GetKeyDown(KeyCode.C)) // c key released
            {
                foreach (GameObject agentInLoop in agents)
                {
                    if (agentInLoop != selectedAgent)
                    {
                        setLoopPrefs(agentInLoop, false);
                        if(selectedAgent.name != "Blue Agent")
                        {
                            agentInLoop.transform.Find("Lantern").gameObject.SetActive(false);
                        }
                        agentInLoop.GetComponent<PlayerController>().updatePeopleInLoop(1);
                    }
                }
                loopers = new List<GameObject>();
                loopers.Add(selectedAgent);
                audioSource.PlayOneShot(ungroupSound);
            }
        }
        else
        {
            Debug.Log("UI is active, not processing input");
            toggleUI(false);
        }

        if (loopers.Count == 0) Debug.LogError("OH NO SOMETHING IS VERY WRONG");

        //guards chasing
        // if (!isTutorial)
        // {
        //     for (int i = 0; i < guards.Length; i++)
        //     {
        //         if (guards[i].GetComponent<AI_Controller>().chasing == true)
        //         {
        //             chaseCount++;
        //         }
        //     }
        //     if (chaseCount == 0)
        //     {
        //         chaseMusic = false;
        //         if (chaseMusic != oldChaseMusic) guards[0].GetComponent<AI_Controller>().suspenseMusic();
        //     }
        //     if (chaseCount > 0)
        //     {
        //         chaseMusic = true;
        //         if (chaseMusic != oldChaseMusic) guards[0].GetComponent<AI_Controller>().chaseMusic();
        //     }
        //     oldChaseMusic = chaseMusic;
        //     chaseCount = 0;
        // }
    }

    public void toggleUI(bool setInUI)
    {
        inUI = setInUI;
    }

    public GameObject raycastForAgent()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 agentToMouse = (mousePos - selectedAgent.transform.position).normalized;
        agentToMouse.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(selectedAgent.transform.position, agentToMouse, maxSwapDistance);

        if (hits.Length == 0)
        {
            Debug.Log("didn't find anything");
            return null;
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Agent") && hit.collider.gameObject != selectedAgent)
            {
                return hit.collider.gameObject;
            }
        }

        Debug.Log("no other agents found in raycast hits");
        return null;
    }

    public void changeSelectedAgent(GameObject newObject)
    {
            //selectedagent
        //if name of newObject is not Blue Agent
        if(newObject.name != "Blue Agent")
        {
            selectedAgent.transform.Find("Lantern").gameObject.SetActive(false);
        }
        selectedAgent = newObject;
        selectedAgent.transform.Find("Lantern").gameObject.SetActive(true);

        //camera
        camController.currentPlayer = newObject.transform;

            //particles
        particles = Instantiate(particleSystem, selectedAgent.transform.position, Quaternion.identity);
        Destroy(particles, 5f);

            //degbug
        Debug.Log("Switched control to " + newObject.name);
    }

    public void updatePointsOfInterest(bool setPOI, GameObject agent)
    {
        PlayerController selectedScript = agent.GetComponent<PlayerController>();
        int layer = setPOI ? 3 : 0; //if setting as a POI, put on layer 3, otherwise on layer 0
        string tagToCheck = "";
        switch(agent.name)
        {
            case "Red Agent":
                tagToCheck = "Goal";
                break;
            case "Blue Agent":
                tagToCheck = "Agent";
                break;
            case "Green Agent":
                tagToCheck = "Guard";
                break;
            case "Orange Agent":
                tagToCheck = "Button";
                break;
            default:
                //this agent has no POI-specific abilities
                break;
        }
        if (tagToCheck != "") selectedScript.setPOIs(layer, tagToCheck);
    }

    public void setLoopPrefs(GameObject agentToUpdate, bool toggleOn) //will toggle POIs and highlights
    {
        //if(selectedAgent.name != "Blue Agent")
        //{
            agentToUpdate.transform.GetChild(0).gameObject.SetActive(toggleOn); //lanter
        //}
        agentToUpdate.transform.GetChild(2).gameObject.SetActive(toggleOn);
        agentToUpdate.transform.GetChild(3).gameObject.SetActive(toggleOn);
        updatePointsOfInterest(toggleOn, agentToUpdate);
    }

    public bool outsideLoopCheck(GameObject agentToCheck)
    {
        foreach(GameObject agentInLoop in loopers)
        {
            if (agentToCheck.name == agentInLoop.name) return false; //found inside loop
        }
        return true;
    }
}
