using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler SharedInstance;

    [SerializeField] private GameObject objectToPool;

    [SerializeField] private int amountToPool = 20;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject objToAdd = Instantiate(objectToPool);
        objToAdd.SetActive(false);
        pooledObjects.Add(objToAdd);
        return objToAdd;
    }
}
