using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{
    private GameManager gameManager;

    [Header("States")]
    public State currentState;
    public WalkState walkState;
    public ChaseState chaseState;
    public AttackState attackState;

    [Header("A.I Settings")]
    public NavMeshAgent agent;
    public GameObject[] enemyTargets;
    public float health;

    [Header("Game Objects")]
    public GameObject projectile;
    public GameObject exit;

    [Header("LayerMask")]
    public LayerMask whatIsGround;
    public LayerMask detectionLayer;

    [Header("Walking")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("Shooting")]
    public float timeBetweenShots;
    public bool justFired;

    [Header("Change States Based On")]
    public float sightRange;
    public float attackRange;
    public bool isTargetInSightRange, isTargetInAttackRange;

    public List<GameObject> multipliersUsed; //List of multipliers hit, so they can be used once only

    private void Awake()
    {
        //Create reference to gameManager to allow common functions.
        gameManager = FindObjectOfType<GameManager>();

        // Populate enemy list based on other team
        if (gameObject.tag == "BlueTeam")
        {
            enemyTargets = gameManager.redTeam;
        }
        else if (gameObject.tag == "RedTeam")
        {
            enemyTargets = gameManager.blueTeam;
        }

        // Declare agent nav mesh
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Use detection layer to see if any enemies are in sight or attach range.
        isTargetInSightRange = Physics.CheckSphere(transform.position, sightRange, detectionLayer);
        isTargetInAttackRange = Physics.CheckSphere(transform.position, attackRange, detectionLayer);

        // Determine and call Current State, assuming game not already won
        if (gameManager.numOfReds > 0)
        {
            HandleStateMachine();
        }
    }

    private void HandleStateMachine()
    {
        if (isTargetInAttackRange) // Target in attack range
        {
            //Update enemy list based on team type
            if (gameObject.tag == "BlueTeam")
            {
                enemyTargets = gameManager.redTeam;
            }
            else if (gameObject.tag == "RedTeam")
            {
                enemyTargets = gameManager.blueTeam;
            }

            //Set current state and remove values from previous states
            currentState = attackState;
            chaseState.chaseTarget = null;
            walkPointSet = false;
        }
        else if (isTargetInSightRange) // Target in sight but out of attack range so chase target
        {
            //Update enemy list based on team type
            if (gameObject.tag == "BlueTeam")
            {
                enemyTargets = gameManager.redTeam;
            }
            else if (gameObject.tag == "RedTeam")
            {
                enemyTargets = gameManager.blueTeam;
            }

            //Set current state and remove values from previous states
            currentState = chaseState;
            attackState.attackTarget = null;
            walkPointSet = false;
        }
        else // Everything out of range so default to walk state
        {
            //Set current state and remove values from previous states
            currentState = walkState; 
            chaseState.chaseTarget = null;
            attackState.attackTarget = null;
        }

        // Question mark at end of current state means if the variable is null it won't call the function
        // If not null then Run Current State will be called from appropriate script (abstract function in State.cs)
        currentState?.RunCurrentState(this);
        
    }

    public void TakeDamage(int damage)
    {
        //Reduce health and destroy self if no more health left
        health -= damage;

        if (health <= 0)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    // Trigger enter to handle Blue Team members colliding into White Team members to get them join their team
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("BlueTeam") && other.gameObject.CompareTag("WhiteTeam")) // If Blue hits White then turn White to Blue
        {
            other.gameObject.tag = "BlueTeam";
            other.gameObject.layer = 8; // Blue team layer value
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }
}
