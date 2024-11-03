using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenadeBehaviour : MonoBehaviour
{
    [Header("Base Variables")]
    public float baseDamage;
    public float minDamage;
    public float blastRadius;
    public bool damageNoFalloff;
    public float damageFalloffStart; //this will be a percentage of the blast radius
    public float explosionDelay;

    [Header("Fire Variables")]
    public bool spawnFire;
    public float spawnedFireDuration;
    public float fireTickSpeed;
    public float fireDuration;
    public float fireDamagePerTick;
    public float fireDensity; //will look at more. for fire balls. will replace with vfx and mvoe the fireball behaivouir to this script and jsut ause a single collider on the grenade
    public float fireExpansionPercentage;
    public bool enemiesInFireSlowed;
    public float fireSlowStrength;

    private bool fireSpawned;

    [Header("Stun Variables")]
    public bool knockbackAndStun;
    public float knockbackForceMultiplier;
    public float stunDuration;


    public bool applyDamageOverTime; //why unless it is made more clear that this is a seperate status effect from fire and is like a poision or something

    [Header("Objects and VFX")]
    public GameObject grenade;
    public GameObject fireVFX;
    public GameObject explosionVFX;

    private List<EnemyStats> affectedEnemies; //try to figure out a better way to do this. this is to prevent the fact that you have multiple colliders on enemies make it so you affect the object multiple times

    private void Start()
    {
        StartCoroutine(ExplosionCountdown());
    }

    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void Explode()
    {
        transform.position = grenade.transform.position;
        explosionVFX.SetActive(true);
        Destroy(grenade);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        affectedEnemies = new List<EnemyStats>();

        for(int a = 0; a < colliders.Length; a++)
        {
            EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
            if (enemy != null)
            {
                if(!affectedEnemies.Contains(enemy))
                {
                    affectedEnemies.Add(enemy);
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    float damage = damageNoFalloff ? baseDamage : CalculateDamage(distance);

                    enemy.TakeDamage(damage);

                    if (knockbackAndStun)
                    {
                        Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                        float knockbackForce = damage * knockbackForceMultiplier;
                        //enemy.ApplyKnockback(knockbackDirection * knockbackForce, knockbackForce);
                        enemy.ApplyStun(stunDuration);
                    }

                    if (applyDamageOverTime)
                    {
                        //enemy.ApplyDamageOverTime(50f, 5f);  // 50 damage over 5 seconds
                    }
                }

            }
        }

        if (spawnFire)
        {
            fireSpawned = true;
            SpawnFireSpheres(); //replaced with VFX
            StartCoroutine(DestroyGrenade(spawnedFireDuration));
        } else {
            StartCoroutine(DestroyGrenade(1.5f));
        }
    }

    private float CalculateDamage(float distance)
    {
        if (distance <= damageFalloffStart)
        {
            return baseDamage;
        }
        else if (distance >= blastRadius)
        {
            return minDamage;
        }
        else
        {
            float t = (distance - damageFalloffStart) / (blastRadius - damageFalloffStart);
            return Mathf.Lerp(baseDamage, minDamage, t);
        }
    }

    private void SpawnFireSpheres() //replaced with VFX
    {
        float radiusMultipler = 1 + (fireExpansionPercentage / 100);
        float multipliedBlastRadius = blastRadius * radiusMultipler;
        int sphereCount = Mathf.CeilToInt(Mathf.PI * multipliedBlastRadius * multipliedBlastRadius * fireDensity);
        //Debug.Log("Spawning " + sphereCount + " fireballs.");

        for (int i = 0; i < sphereCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * blastRadius;
            Vector3 randomPosition = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);

            Instantiate(fireVFX, randomPosition, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        if(fireSpawned)
        {
            DealFireDamage();
        }
    }

    private void DealFireDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        for(int a = 0; a < colliders.Length; a++)
        {
            //Debug.Log(colliders[a]);
            EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.GetFireData(fireTickSpeed, fireDuration, fireDamagePerTick);

                if (enemiesInFireSlowed)
                {
                    enemy.ApplySlow(fireDuration, fireSlowStrength);
                }
            }
        }
    }

    private IEnumerator DestroyGrenade(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
