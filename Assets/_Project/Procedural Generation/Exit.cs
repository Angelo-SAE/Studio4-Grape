using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObjectObject proceduralGenerationObject;
    [SerializeField] private GameObjectObject exitObject;

    private void Start()
    {
        exitObject.value = gameObject;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            proceduralGenerationObject.value.GetComponent<MissionExit>().Exit();
        }
    }
}
