using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCoreBehaviour : MonoBehaviour
{
    private float aoeRadius;
    private float duration;
    private bool willExplode;
    private float explosionDamage;
    public bool applyVulnerable = false;
    public float damageIncreasePercentage = 0f;
    public float vulnerableDuration = 0f;
    public bool applyStun = false;
    public bool applyWeaken = false;
    public float weakenPercentage = 0f;
    public bool strongerMagneticEffect = false;

    private bool isActivated = false;
    private bool hasLanded = false;

    private List<Enemy> affectedEnemies = new List<Enemy>();

    void Start()
    {
        StartCoroutine(SelfDestructIfNotLanded(5f));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActivated)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                hasLanded = true;
                ActivateCore();
            }
        }
    }

    void ActivateCore()
    {
        isActivated = true;

        StartCoroutine(CoreLifetime());

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void Initialize(float radius, float duration, bool explodeAtEnd, float explosionDamage)
    {
        this.aoeRadius = radius;
        this.duration = duration;
        this.willExplode = explodeAtEnd;
        this.explosionDamage = explosionDamage;
    }

    void Update()
    {
        if (isActivated)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
            foreach (Collider collider in colliders)
            {
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    if (!affectedEnemies.Contains(enemy))
                    {
                        affectedEnemies.Add(enemy);
                        if (applyVulnerable)
                        {
                            enemy.ApplyVulnerable(damageIncreasePercentage, vulnerableDuration > 0 ? vulnerableDuration : duration);
                        }
                        if (applyWeaken)
                        {
                            enemy.ApplyWeaken(weakenPercentage);
                        }
                    }
                    PullEnemy(enemy);
                }
            }
        }
    }

    void PullEnemy(Enemy enemy)
    {
        Vector3 direction = (transform.position - enemy.transform.position).normalized;
        //direction.y = 0;
        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        //float pullStrength = strongerMagneticEffect ? 1.5f - (distance / aoeRadius) : 1.25f - (distance / aoeRadius);
        float pullStrength = strongerMagneticEffect ? 1.5f - (0) : 1.25f - (0);

        Vector3 pullForce = direction * pullStrength * enemy.GetMoveSpeed() * 0.25f;
        enemy.AddForce(pullForce);
    }

    IEnumerator CoreLifetime()
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

    IEnumerator SelfDestructIfNotLanded(float timeToSelfDestruct)
    {
        yield return new WaitForSeconds(timeToSelfDestruct);
        if (!hasLanded)
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
                if (applyStun)
                {
                    enemy.Stun(3f); // Stun duration can be adjusted as needed
                }
            }
        }
    }
}
