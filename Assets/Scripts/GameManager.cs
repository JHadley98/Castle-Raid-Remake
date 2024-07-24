using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject exit;
    private GateControl gateControl;

    public GameObject[] redTeam;
    public GameObject[] blueTeam;
    public int numOfReds;
    public int numOfBlues;
    private int bluesInEndZone;

    [Header("UI Elements")]
    [SerializeField] private GameObject LevelWonUI;
    [SerializeField] private GameObject LevelLostUI;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        //Create reference to gate control to allow game manager to open gates.
        gateControl = FindObjectOfType<GateControl>();

        //Get initial red team and blue team arrays
        redTeam = GameObject.FindGameObjectsWithTag("RedTeam");
        blueTeam = GameObject.FindGameObjectsWithTag("BlueTeam");
        numOfReds = redTeam.Length;
        numOfBlues = blueTeam.Length;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Refresh blue team
        blueTeam = GameObject.FindGameObjectsWithTag("BlueTeam");
        numOfBlues = blueTeam.Length;

        //If there are reds left then refresh red team and continue, otherwise enter final win stage (blues go through exit)
        if (numOfReds > 0)
        {
            redTeam = GameObject.FindGameObjectsWithTag("RedTeam");
            numOfReds = redTeam.Length;

            // check if all blues are dead
            if (numOfBlues == 0)
            {
                LostLevel(); //Lose
            }
        }
        else
        {
            // Final win stage

            //Open Gates
            gateControl.OpenGates();

            //Check if all surviving blues are in end zone
            bluesInEndZone = 0;
            foreach (GameObject blueAI in blueTeam)
            {
                if (DistanceBetweenPoints(blueAI.transform.position, exit.transform.position) < 3f)
                {
                    bluesInEndZone++;
                }
                else
                {
                    blueAI.GetComponent<AIController>().agent.destination = exit.transform.position;
                }
            }
            if (bluesInEndZone == numOfBlues)
            {
                WonLevel();
            }
        }
    }

    public void WonLevel()
    {
        //Game won. Turn on canvas and stop game playing
        canvasUI.SetActive(true);
        LevelWonUI.SetActive(true);
        gameOverText.text = "Congratulations";
        Time.timeScale = 0f;
    }

    public void LostLevel()
    {
        //Game Lost. Turn on canvas and stop game playing
        canvasUI.SetActive(true);
        LevelLostUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    // Function to calculate distance between 2 vectors. Used in mulitple scripts.
    public float DistanceBetweenPoints(Vector3 vector1, Vector3 vector2)
    {
        Vector3 diff = vector1 - vector2;
        return Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.y, 2) + Mathf.Pow(diff.z, 2));
    }
}
