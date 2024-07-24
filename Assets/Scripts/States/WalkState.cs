using UnityEngine;

public class WalkState : State
{
    // public ChaseState chaseState;
    private Vector3 previousPosition;
    private GameManager gameManager;

    private void Awake()
    {
        //Create reference to gameManager to allow common functions.
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void RunCurrentState(AIController controller)
    {
        // Only blue team does general walking
        if (controller.gameObject.tag == "BlueTeam")
        {
            //Update walkpoint if required.
            if (controller.transform.position.z - previousPosition.z <= 0.2f) // Prevent path finding going backwards 
            {
                SearchWalkPoint(controller);
            }
            else if (!controller.walkPointSet)   // If walk point not set find one
            {
                SearchWalkPoint(controller);
            }
            else if (gameManager.DistanceBetweenPoints(transform.position, controller.walkPoint) < 1f) // If nearly at walk point get new destination
            {
                SearchWalkPoint(controller);
            }

            //Set destination if good walk point available
            if (controller.walkPointSet)
            {
                // Pass walk point to destination for actor movement
                controller.agent.destination = controller.walkPoint;
            }

            //Log position for backward travel check abouve.
            previousPosition = controller.transform.position;
        }
    }

    private void SearchWalkPoint(AIController controller)
    {
        // Set Walk Point
        controller.walkPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.0f);

        // Check walk point is directly visible, if not try slightly left then slightly right to get a better destination.
        if (!Physics.Raycast(transform.position, controller.walkPoint, 2f,8))
        {
            controller.walkPointSet = true;
        }
        else
        {
            controller.walkPoint = new Vector3(transform.position.x + 2.0f, transform.position.y, transform.position.z + 2.0f);
            if (!Physics.Raycast(transform.position, controller.walkPoint, 3f,8))
            {
                controller.walkPointSet = true;
            }
            else
            {
                controller.walkPoint = new Vector3(transform.position.x - 2.0f, transform.position.y, transform.position.z + 2.0f);
                if (!Physics.Raycast(transform.position, controller.walkPoint, 3f,8))
                {
                    controller.walkPointSet = true;
                }
                else
                {
                    controller.walkPointSet = false;
                }
            }
        }
    }
}
