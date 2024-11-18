using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectListObject spawnerList;
    [SerializeField] private MonsterSetsObject monsterSets;
    [SerializeField] private Vector3Object playerPosition;

    [Header("Spawner Variables")]
    [SerializeField] private float creditProductionSpeed;
    [SerializeField] private float spawnCredits;
    [SerializeField] private int spawnAmount;

    [SerializeField] private int spawnCost;
    private bool findWhatToSpawn;
    private Vector3[] closestSpawner;
    private GameObject[] setsToSpawn;
    private Vector3 closestHolder;
    private GameObject tempHolder;


    private void Update()
    {
        if(!findWhatToSpawn)
        {
            SelectWhatToSpawn();
        }

        spawnCredits += (Time.deltaTime * creditProductionSpeed);

        if(spawnCredits > spawnCost)
        {
            Destroy(tempHolder);
            tempHolder = new GameObject(); // temp code for testing
            GetSpawnPositions();
            SpawnMonstersAroundPlayer();
        }
    }

    private void SelectWhatToSpawn()
    {
        findWhatToSpawn = true;
        setsToSpawn = new GameObject[spawnAmount];
        for(int a = 0; a < spawnAmount; a++)
        {
            int temp = Random.Range(0, monsterSets.sets.Length);
            setsToSpawn[a] = monsterSets.sets[temp];
            spawnCost += monsterSets.spawnCosts[temp];
        }
    }

    private void SpawnRandomMonsters()
    {
        for(int a = 0; a < 10; a++)
        {
            Instantiate(monsterSets.sets[Random.Range(0, monsterSets.sets.Length)], spawnerList.value[Random.Range(0, spawnerList.value.Count)].transform.position, Quaternion.Euler(Vector3.zero), tempHolder.transform);
        }
    }

    private void GetSpawnPositions()
    {
        closestSpawner = new Vector3[setsToSpawn.Length];
        for(int a = 0; a < closestSpawner.Length; a++)
        {
            closestSpawner[a] = new Vector3(1000, 1000, 1000);
        }


        for(int a = 0; a < spawnerList.value.Count; a++)
        {
            closestHolder = spawnerList.value[a].transform.position;
            for(int b = 0; b < closestSpawner.Length; b++)
            {
                closestSpawner[b] = FindClosest(closestSpawner[b], closestHolder);
            }
        }
    }

    private Vector3 FindClosest(Vector3 positionOne, Vector3 positionTwo)
    {
        if(Vector3.Distance(positionOne, playerPosition.value) > Vector3.Distance(positionTwo, playerPosition.value))
        {
            closestHolder = positionOne;
            return positionTwo;
        } else {
            return positionOne;
        }
    }

    private void SpawnMonstersAroundPlayer()
    {
        for(int a = 0; a < setsToSpawn.Length; a++)
        {
            Instantiate(setsToSpawn[a], closestSpawner[a], Quaternion.Euler(Vector3.zero), tempHolder.transform);
        }
        spawnCredits = 0;
        spawnCost = 0;
        findWhatToSpawn = false;
    }
}
