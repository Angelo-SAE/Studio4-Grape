using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallandBigEnemy : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Enemy Behaviours")]
    [SerializeField] private ChaseBehaviour chaseBehaviour;
    [SerializeField] private AttackBehaviour attackBehaviour;

    [Header("Extra")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float detectionRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDuration;
    [SerializeField] private bool playerInRange;

    private bool attackingPlayer;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackAnimation;

    private void Update()
    {
        if(playerInRange)
        {
            if(!attackingPlayer)
            {
                animator.Play("RunForward");
                chaseBehaviour.RunBehaviour();
                CheckForAttack();
                CheckForPlayerLeavingRange();
            }
        } else {
            CheckForPlayer();
            animator.Play("Idle");
        }
    }

    private void CheckForPlayer()
    {
        if(Vector3.Distance(transform.position, playerObject.value.transform.position) < detectionRange)
        {
            RaycastHit hit;
            Vector3 tempDirection = playerObject.value.transform.position - transform.position;
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), new Vector3(tempDirection.x, 0f, tempDirection.z).normalized, out hit, detectionRange, enemyLayer);
            if(hit.collider is not null)
            {
                if(hit.collider.tag == "Player")
                {
                    playerInRange = true;
                }
            }
        }
    }

    private void CheckForPlayerLeavingRange()
    {
        if(Vector3.Distance(transform.position, playerObject.value.transform.position) > chaseRange)
        {
            playerInRange = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void CheckForAttack()
    {
        if(Vector3.Distance(transform.position, playerObject.value.transform.position) < attackRange)
        {
            AttackPlayer();
            attackingPlayer = true;
        }
    }

    private void AttackPlayer()
    {
        animator.Play(attackAnimation);
        rb.velocity = Vector3.zero;
        Invoke("Attack", attackDelay);
        Invoke("StopAttack", attackDuration);
    }

    private void Attack()
    {
        attackBehaviour.RunBehaviour();
    }

    private void StopAttack()
    {
        attackingPlayer = false;
        animator.Play("Idle");
    }
}
