using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermobaricGrenadeBehaviour : MonoBehaviour
{
    [Header("Base Variables")]
    public float explosionDamage;
    public float blastRadius;
    [SerializeField] private float explosionDelay;

    [Header("Fire Variables")]
    public bool spawnFire;
    public float fireDuration;
    public float fireDamagePerTick;
    [SerializeField] private float fireTickSpeed;
    [SerializeField] private float fireBurnDuration;
    [SerializeField] private float fireDensity;

    private bool fireSpawned;

    [Header("Bleed Variables")]
    public bool applyBleed;
    public float bleedDamage;
    public float bleedTickSpeed;

    [Header("Stun Variables")]
    public bool stun;
    public float stunDuration;

    [Header("Slow Variables")]
    public bool slow;
    public float slowDuration;
    [SerializeField] private float slowStrength;

    [Header("Vulnerable Variables")]
    public bool vulnerable;
    public float vulnerableDuration;




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
                    float damage = explosionDamage;

                    enemy.TakeDamage(damage, Color.yellow);

                    if (stun)
                    {
                        enemy.ApplyStun(stunDuration);
                    }

                    if(slow)
                    {
                        enemy.ApplySlow(slowDuration, slowStrength);
                    }

                    if(vulnerable)
                    {
                        enemy.ApplyVulnerable(vulnerableDuration);
                    }

                    if (applyBleed)
                    {
                        enemy.ApplyBleed(bleedTickSpeed, bleedDamage);
                    }
                }

            }
        }

        if (spawnFire)
        {
            fireSpawned = true;
            SpawnFireVFX();
            StartCoroutine(DestroyGrenade(fireDuration));
        } else {
            StartCoroutine(DestroyGrenade(1.5f));
        }
    }

    private void SpawnFireVFX()
    {
        int sphereCount = Mathf.CeilToInt(Mathf.PI * blastRadius * blastRadius * fireDensity);

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
            EnemyStats enemy = colliders[a].GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.GetFireData(fireTickSpeed, fireBurnDuration, fireDamagePerTick);
            }
        }
    }

    private IEnumerator DestroyGrenade(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
