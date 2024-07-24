using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public GameObject chaseTarget;
    private GameManager gameManager;

    private void Awake()
    {
        //Create reference to gameManager to allow common functions.
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void RunCurrentState(AIController controller)
    {
        //Get new target if one is not currently set or if the target is destroyed
        if (!chaseTarget)
        {
            // Loop through all enemy in enemy targets
            foreach (GameObject enemy in controller.enemyTargets)
            {
                // Check if enemy is in sight range 
                if (enemy) //Skip if enemy in list has just been destroyed
                {
                    if (gameManager.DistanceBetweenPoints(enemy.transform.position, controller.gameObject.transform.position) <= controller.sightRange + 0.5f) // 0.5 allows for size of capusle to middle
                    {
                        //Set target and exit loop once a suitable enemy has been found
                        chaseTarget = enemy;
                        break;
                    }
                }

            }
        }

        // Pass walk point to destination for actor movement
        controller.agent.destination = chaseTarget.transform.position;
    }
}
