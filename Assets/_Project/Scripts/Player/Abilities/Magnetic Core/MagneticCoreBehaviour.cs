using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCoreBehaviour : MonoBehaviour
{
    [Header("Ground Check Variables")]
    [SerializeField] private float groundCheckLength;
    [SerializeField] private LayerMask groundLayers;

    [Header("General Variables")]
    [SerializeField] private Rigidbody rb;
    private bool isActivated = false;
    private bool hasLanded = false;

    private List<EnemyStats> affectedEnemies; //try to figure out a better way to do this. this is to prevent the fact that you have multiple colliders on enemies make it so you affect the object multiple times

    [Header("Base Stats")]
    public float aoeRadius;
    public float duration;

    [Header("Explosions")]
    public bool bigExplode;
    public bool miniExplosions;
    public float explosionDamage;
    public float explosionDelay;

    private float currentExplosionDelay;

    [Header("Vunerable")]
    public bool vulnerable;
    public float vulnerableDuration;

    [Header("Stun")]
    public bool stun;
    public float stunDuration;

    [Header("Weaken")]
    public bool weaken;
    public float weakenStrength;
    public float weakenDuration;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckLength);
    }

    private void Start()
    {
        StartCoroutine(SelfDestructIfNotLanded(5f));
        explosionDelay = (duration / Mathf.Round(duration/explosionDelay)) - 0.1f;
    }

    private IEnumerator SelfDestructIfNotLanded(float timeToSelfDestruct)
    {
        yield return new WaitForSeconds(timeToSelfDestruct);
        if(!hasLanded)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!hasLanded)
        {
            CheckForGround();
        }

        if(isActivated)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
            affectedEnemies = new List<EnemyStats>();
            for(int a = 0; a < colliders.Length; a++)
            {
                EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
                if(enemy != null)
                {
                    if(!affectedEnemies.Contains(enemy))
                    {
                        affectedEnemies.Add(enemy);
                        if(vulnerable)
                        {
                            enemy.ApplyVulnerable(vulnerableDuration);
                        }
                        if(weaken)
                        {
                            enemy.ApplyWeaken(weakenStrength, weakenDuration);
                        }
                        SetPullForce(enemy);
                    }
                }
            }

            if(miniExplosions)
            {
                currentExplosionDelay += Time.deltaTime;
                if(currentExplosionDelay >= explosionDelay)
                {
                    currentExplosionDelay -= explosionDelay;
                    MiniExplode();
                }
            }
        }
    }

    private void MiniExplode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
        affectedEnemies = new List<EnemyStats>();

        for(int a = 0; a < colliders.Length; a++)
        {
            EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
            if(enemy != null)
            {
                if(!affectedEnemies.Contains(enemy))
                {
                    affectedEnemies.Add(enemy);
                    if (enemy != null)
                    {
                        enemy.TakeDamage(explosionDamage);
                    }
                }
            }
        }
    }

    private void CheckForGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, groundCheckLength, groundLayers))
        {
            hasLanded = true;
            ActivateCore();
        }
    }

    private void ActivateCore()
    {
        isActivated = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.freezeRotation = true;
        StartCoroutine(CoreLifetime());
    }

    private void SetPullForce(EnemyStats enemy)
    {
        Vector3 direction = (transform.position - enemy.transform.position).normalized;

        enemy.pullForce = direction;
    }

    private IEnumerator CoreLifetime()
    {
        yield return new WaitForSeconds(duration);
        if (bigExplode)
        {
            Explode();
        }
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
        affectedEnemies = new List<EnemyStats>();

        for(int a = 0; a < colliders.Length; a++)
        {
            EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
            if(enemy != null)
            {
                if(!affectedEnemies.Contains(enemy))
                {
                    affectedEnemies.Add(enemy);
                    if (enemy != null)
                    {
                        enemy.TakeDamage(explosionDamage);
                        if (stun)
                        {
                            enemy.ApplyStun(stunDuration);
                        }
                    }
                }
            }
        }
    }


}
