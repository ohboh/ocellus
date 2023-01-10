using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollow : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform player;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    //Patroling

    [SerializeField] Vector3 walkPoint;
    [SerializeField] bool walkPointSet;
    [SerializeField] float walkPointRange;
    [SerializeField] float maxDelay;
    [SerializeField] float findingDelay;

    [SerializeField] Vector3 distanceToWalkPoint;



    //Attacking
    bool alreadyJumped;

    //States
    [SerializeField] float sightRange, attackRange;

    [SerializeField] bool playerInSightRange, playerInAttackRange;

    private void Update()

    {

        //Check for sight and attack range

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && findingDelay <= 0) {Patroling();}

        if (!playerInSightRange && !playerInAttackRange && findingDelay > 0) {ChasePlayer();}

        if (playerInSightRange && !playerInAttackRange) {ChasePlayer(); findingDelay = maxDelay;}

        if (playerInAttackRange && playerInSightRange) JumpPlayer();

    }



    private void Patroling()

    {

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        else
        {
            agent.SetDestination(walkPoint);
        }

        distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()

    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }



    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        findingDelay -= Time.deltaTime;
    }

    private void JumpPlayer()

    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyJumped)

        {
            ///Attack code here
            
            ///End of attack code
            alreadyJumped = true;
        }

    }

    private void ResetAttack()

    {
        alreadyJumped = false;
    }

    private void DestroyEnemy()

    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()

    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

}
