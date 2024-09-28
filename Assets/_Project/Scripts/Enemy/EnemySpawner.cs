using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy To Spawn")]
    [SerializeField] private GameObject enemyToSpawn;

    [Header("Spawner Variables")]
    [SerializeField] private float spawnRadius;
    [SerializeField] private int amountToSpawn;

    private bool isSpawning;

    [Header("Spawn Portal")]
    [SerializeField] private GameObject portal;
    [SerializeField] private float portalSize;


    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && !isSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        for(int a = 100; a > 0; a--)
        {
            portal.transform.localScale = new Vector3(portalSize/a, portal.transform.localScale.y, portalSize/a);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
        Spawn();
        yield return new WaitForSeconds(0.5f);

        for(int a = 1; a < 100; a++)
        {
            portal.transform.localScale = new Vector3(portalSize/a, portal.transform.localScale.y, portalSize/a);
            yield return new WaitForSeconds(0.01f);
        }
        portal.transform.localScale = new Vector3(0f, portal.transform.localScale.y, 0f);

        isSpawning = false;
    }

    private void Spawn()
    {
        for(int a = 0; a < amountToSpawn; a++)
        {
            Instantiate(enemyToSpawn, transform);
        }

    }
}
