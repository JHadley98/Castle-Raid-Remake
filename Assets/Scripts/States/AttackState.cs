using UnityEngine;

public class AttackState : State
{
    public GameObject attackTarget;
    private float timeSinceFired;
    private GameManager gameManager;

    private void Awake()
    {
        //Create reference to gameManager to allow common functions.
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void RunCurrentState(AIController controller)
    {
        //If attack target is not set or is destroyed, then get a new one.
        if (!attackTarget)
        {
            if (controller.enemyTargets.Length > 0)
            {
                // Loop through each enemy in enemy targets
                foreach (GameObject enemy in controller.enemyTargets)
                {
                    // Check if enemy is in attack range 
                    if (enemy)
                    {
                        if (gameManager.DistanceBetweenPoints(enemy.transform.position, controller.gameObject.transform.position) < controller.attackRange + 0.5f) // 0.5 allows for size of capusle to middle
                        {
                            //Set target and exit out of loop (as already have a suitable target)
                            attackTarget = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // Make sure ai faces the target
        controller.transform.LookAt(attackTarget.transform.position);

        // Move towards target if far away then stop when close to allow for shooting
        if (gameManager.DistanceBetweenPoints(controller.transform.position, attackTarget.transform.position) > 6)
        {
            controller.agent.destination = controller.transform.position + controller.transform.forward;
        }
        else
        {
            controller.agent.destination = controller.transform.position; //Stop movement by setting destination to self
        }

        if (attackTarget)
        {
            // Check if just fired
            if (!controller.justFired)
            {
                // Firing code
                Rigidbody rb = Instantiate(controller.projectile, controller.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(controller.transform.forward * 32f, ForceMode.Impulse);
                rb.GetComponent<Bullet>().myTeam = controller.gameObject.tag; //add tag to bullet, so it knows what team it is on

                controller.justFired = true;
                timeSinceFired = 0f;
            }
            else
            {
                UpdateTimeSinceFired(controller);
            }
        }

    }

    private void UpdateTimeSinceFired(AIController controller)
    {
        timeSinceFired += Time.deltaTime;

        // If time is more than time between shots then set just fired to false, allowing the next shot to be fired
        if (timeSinceFired >= controller.timeBetweenShots)
        {
            controller.justFired = false;
        }
    }
}
