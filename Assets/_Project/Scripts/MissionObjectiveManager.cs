using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionObjectiveManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text missionTextDisplay;

    [SerializeField] private BoolObject missionComplete;

    [Header("Exit Mission")]
    [SerializeField] private string exitMissionText;
    [SerializeField] private UnityEvent exitMissionEvent;
    [SerializeField] private GameObjectObject exitObject;

    [Header("Kill Enemies Mission")]
    [SerializeField] private IntObject enemiesKilled;
    [SerializeField] private string killMissionText;
    [SerializeField] private int amountToKill;

    private int currentEnemies;
    private bool killEnemiesMission;

    private void Start()
    {
        missionComplete.value = false;
        SelectAndDisplayMission();
    }

    public void SelectAndDisplayMission()
    {
        currentEnemies = enemiesKilled.value;
        killEnemiesMission = true;
    }

    private void Update()
    {
        if(killEnemiesMission)
        {
            CheckForKilledEnemies();
        }
    }

    private void CheckForKilledEnemies()
    {
        missionTextDisplay.text = killMissionText + " (" + (enemiesKilled.value - currentEnemies) + "/" + amountToKill + ")";
        if(enemiesKilled.value - currentEnemies >= amountToKill)
        {
            killEnemiesMission = false;
            ExitMission();
        }
    }

    private void ExitMission()
    {
        missionTextDisplay.text = exitMissionText;
        exitObject.value.SetActive(true);
    }
}
