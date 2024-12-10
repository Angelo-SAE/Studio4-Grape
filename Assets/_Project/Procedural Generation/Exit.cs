using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObjectObject proceduralGenerationObject;
    [SerializeField] private GameObjectObject exitObject;

    private bool collided;

    private void Start()
    {
        exitObject.value = gameObject;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && !collided)
        {
            collided = true;
            proceduralGenerationObject.value.GetComponent<MissionExit>().Exit();
        }
    }
}
