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
    public float pullStrength;
    public float aoeRadius;
    public float duration;
    public bool strongerMagneticEffect = false;

    [Header("Explosion")]
    public bool willExplode;
    public float explosionDamage;

    [Header("Vunerable")]
    public bool applyVulnerable = false;
    public float damageIncreasePercentage = 0f;
    public float vulnerableDuration = 0f;

    [Header("Stun")]
    public bool applyStun = false;
    public float stunDuration;

    [Header("Weaken")]
    public bool applyWeaken = false;
    public float weakenPercentage = 0f;
    public float weakenDuration;

    public void Initialize(float radius, float duration, bool explodeAtEnd, float explosionDamage) //will remove
    {
        this.aoeRadius = radius;
        this.duration = duration;
        this.willExplode = explodeAtEnd;
        this.explosionDamage = explosionDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckLength);
    }

    private void Start()
    {
        StartCoroutine(SelfDestructIfNotLanded(5f));
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
                //Debug.Log(colliders[a]);
                if(enemy != null)
                {
                    if(!affectedEnemies.Contains(enemy))
                    {
                        affectedEnemies.Add(enemy);
                        if(applyVulnerable)
                        {
                            enemy.ApplyVulnerable(damageIncreasePercentage, vulnerableDuration);
                        }
                        if (applyWeaken)
                        {
                            enemy.ApplyWeaken(weakenPercentage, weakenDuration);
                        }
                        SetPullForce(enemy);
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
        //direction.y = 0;
        //Debug.Log(direction * pullStrength);
        enemy.pullForce = direction;
    }

    private IEnumerator CoreLifetime()
    {
        //Debug.Log("Waiting " + duration + " seconds");
        yield return new WaitForSeconds(duration);
        if (willExplode)
        {
            Explode();
        }
        //Debug.Log("Destroying magnetic core");
        Destroy(gameObject);
    }



    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider collider in colliders)
        {
            EnemyStats enemy = collider.GetComponentInParent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
                if (applyStun)
                {
                    enemy.ApplyStun(3f); // Stun duration should be set by the ability
                }
            }
        }
    }
}
