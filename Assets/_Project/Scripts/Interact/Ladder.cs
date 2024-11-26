using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Interactable Variables")]
    [SerializeField] public bool hasPopup;

    [Header("Ladder Variables")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform topEndPosition;
    [SerializeField] private float ladderHeight;
    [SerializeField] private float bottomStartHeight;
    [SerializeField] private float topStartHeight;

    public bool CheckIfInteractable()
    {
        return true;
    }

    public void Interact()
    {
        StartLadderClimb();
    }

    public void AltInteract() {}

    private void StartLadderClimb()
    {
        if ((transform.position.y + (ladderHeight / 2)) - playerObject.value.transform.position.y > 0)
        {
            Vector3 startPosition = new Vector3(
                playerPosition.position.x,
                transform.position.y + bottomStartHeight,
                playerPosition.position.z
            );
            StartCoroutine(TransitionToLadder(
                playerObject.value.transform,
                startPosition,
                playerPosition.localEulerAngles,
                false
            ));
        }
        else
        {
            Vector3 startPosition = new Vector3(
                playerPosition.position.x,
                transform.position.y + ladderHeight - topStartHeight,
                playerPosition.position.z
            );
            StartCoroutine(TransitionToLadder(
                playerObject.value.transform,
                startPosition,
                playerPosition.localEulerAngles,
                true
            ));
        }
    }

    // Coroutine for smooth position and rotation transition
    private IEnumerator TransitionToLadder(
        Transform playerTransform,
        Vector3 targetPosition,
        Vector3 targetRotation,
        bool isAtTop
    )
    {
        float transitionDuration = 0.5f; // Time for smooth transition
        float elapsedTime = 0f;

        Vector3 initialPosition = playerTransform.position;
        Vector3 initialRotation = playerTransform.localEulerAngles;

        // Smoothly interpolate position and rotation
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            playerTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            playerTransform.localEulerAngles = Vector3.Lerp(initialRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap to final position and rotation
        playerTransform.position = targetPosition;
        playerTransform.localEulerAngles = targetRotation;

        // Start ladder climbing after transition
        playerObject.value.GetComponent<ThirdPersonMovement>().StartLadderClimb(
            targetPosition,
            topEndPosition.position,
            targetRotation,
            topEndPosition.localEulerAngles,
            transform.position.y + ladderHeight - topStartHeight + 0.2f,
            transform.position.y + 0.1f,
            isAtTop
        );
    }



}
