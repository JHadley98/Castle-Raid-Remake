using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameManager gameManager;
    public Vector3 offset;
    public Vector3 velocity;
    public float smoothTime = 0.5f;


    private void Awake()
    {
        //Create reference to gameManager to allow common functions.
        gameManager = FindObjectOfType<GameManager>();
    }

    private void LateUpdate() //Late update to allow for character movement before camera movement
    {
        Movement();
        KillFallenBehind();
    }

    private void Movement()
    {
        // If there are blues remaining then move camera to the center of the group
        if (gameManager.numOfBlues > 0)
        {
            Vector3 centerPoint = GetCenterPoint();

            Vector3 newPosition = centerPoint + offset;

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

    }
    private Vector3 GetCenterPoint()
    {
        //Create bounds of all Blue AI, staring with first one in array
        Bounds bounds = new Bounds(gameManager.blueTeam[0].transform.position, Vector3.zero);
        //Loop through the rest of the array to add to the bounds
        for (int i = 1; i < gameManager.blueTeam.Length; i++)
        {
            bounds.Encapsulate(gameManager.blueTeam[i].transform.position);
        }

        return bounds.center;
    }

    private void KillFallenBehind()
    {
        //Calculate bottom of screen based on camera view then destroy any reds or blues left behind that limit
        float cameraLimit = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).z;

        foreach (GameObject AI in gameManager.redTeam)
        {
            if(AI.transform.position.z < cameraLimit)
            {
                Destroy(AI);
            }
        }
        foreach (GameObject AI in gameManager.blueTeam)
        {
            if (AI.transform.position.z < cameraLimit)
            {
                Destroy(AI);
            }
        }
    }
}
