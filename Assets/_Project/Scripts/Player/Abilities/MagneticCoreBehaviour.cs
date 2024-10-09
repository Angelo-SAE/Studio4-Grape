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
    public bool vulnerablePersists = false;

    private bool isActivated = false;

    private List<Enemy> affectedEnemies = new List<Enemy>();

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActivated)
        {
            
            if (collision.gameObject.CompareTag("Ground"))
            {
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
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        StartCoroutine(CoreLifetime());


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
                            enemy.ApplyVulnerable(damageIncreasePercentage, vulnerablePersists ? -1f : duration);
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
        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        
        float pullStrength = 1.25f - (distance / aoeRadius);

        
        Vector3 pullForce = direction * pullStrength * enemy.GetMoveSpeed();
        enemy.AddForce(pullForce);
    }

    IEnumerator CoreLifetime()
    {
        Debug.Log("Waiting " + duration + " seconds");
        yield return new WaitForSeconds(duration);
        if (willExplode)
        {
            Explode();
        }
        Debug.Log("Destroying magnetic core");
        Destroy(gameObject);
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
            }
        }
        
    }
}
