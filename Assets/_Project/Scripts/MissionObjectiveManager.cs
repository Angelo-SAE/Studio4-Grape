using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionObjectiveManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text killMissionTextDisplay;
    [SerializeField] private TMP_Text switchMissionTextDisplay;

    [SerializeField] private BoolObject missionComplete;

    [Header("Exit Mission")]
    [SerializeField] private string exitMissionText;
    [SerializeField] private UnityEvent exitMissionEvent;
    [SerializeField] private GameObjectObject exitObject;

    private bool exitMission;

    [Header("Kill Enemies Mission")]
    [SerializeField] private IntObject enemiesKilled;
    [SerializeField] private string killMissionText;
    [SerializeField] private int amountToKill;

    private int currentEnemies;
    private bool killEnemiesMission;

    [Header("Switch Mission")]
    [SerializeField] private IntObject switchCount;
    [SerializeField] private string switchMissionText;
    [SerializeField] private int switchAmount;

    private bool switchMission;

    private void Start()
    {
        switchCount.value = 0;
        missionComplete.value = false;
        SelectAndDisplayMission();
    }

    public void SelectAndDisplayMission()
    {
        currentEnemies = enemiesKilled.value;
        killEnemiesMission = true;

        switchCount.value = 0;
        switchMission = true;
    }

    private void Update()
    {
        if(killEnemiesMission)
        {
            CheckForKilledEnemies();
        }

        if(switchMission)
        {
            CheckForSwitches();
        }

        if(!exitMission && !switchMission && !killEnemiesMission)
        {
            exitMission = false;
            ExitMission();
        }
    }

    private void CheckForKilledEnemies()
    {
        killMissionTextDisplay.text = killMissionText + " (" + (enemiesKilled.value - currentEnemies) + "/" + amountToKill + ")";
        if(enemiesKilled.value - currentEnemies >= amountToKill)
        {
            killEnemiesMission = false;
        }
    }

    private void CheckForSwitches()
    {
        switchMissionTextDisplay.text = switchMissionText + " (" + switchCount.value + "/" + switchAmount + ")";
        if(switchCount.value == switchAmount)
        {
            switchMission = false;
        }
    }

    private void ExitMission()
    {
        killMissionTextDisplay.text = exitMissionText;
        switchMissionTextDisplay.text = "";
        exitObject.value.SetActive(true);
    }
}
