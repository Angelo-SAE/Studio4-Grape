using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UpgradeMenu : MonoBehaviour //add skip button
{
    //Upgrade Order : A AA AAA AAB AB ABA ABB
    //Upgrade Order : 0 1   2   3  4   5   6

    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject upgradeMenuObject;

    [Header("Ability 1 Upgrade Paths")]
    [SerializeField] private ThermobaricGrenade grenade;
    [SerializeField] private UpgradePath[] abilityOneUpgrades;

    [Header("Ability 2 Upgrade Paths")]
    [SerializeField] private MagneticCore magneticCore;
    [SerializeField] private UpgradePath[] abilityTwoUpgrades;

    [Header("Ability 3 Upgrade Paths")]
    [SerializeField] private CombatStim combatStim;
    [SerializeField] private UpgradePath[] abilityThreeUpgrades;

    [Header("General")]
    private List<Upgrade> availableUpgrades;
    private Upgrade[] upgradeSelection;
    private int[] currentRandoms;

    [Header("Upgrade UI")]
    [SerializeField] private Button[] upgradeButton;
    [SerializeField] private TMP_Text[] upgradeName;
    [SerializeField] private TMP_Text[] upgradeDescription;
    [SerializeField] private Image[] upgradeIcon;

    [Header("Events")]
    [SerializeField] private UnityEvent OnAwake;
    [SerializeField] private UnityEvent OnEnableMenu;
    [SerializeField] private UnityEvent OnUpgrade;

    private bool first;

    private void Start()
    {
        upgradeMenuObject.value = gameObject;
        availableUpgrades = new List<Upgrade>();
        //add first upgrades from each path to available upgrades
        for(int a = 0; a < abilityOneUpgrades.Length; a++)
        {
            availableUpgrades.Add(abilityOneUpgrades[a].upgrade[0]);
        }
        for(int a = 0; a < abilityTwoUpgrades.Length; a++)
        {
            availableUpgrades.Add(abilityTwoUpgrades[a].upgrade[0]);
        }
        for(int a = 0; a < abilityThreeUpgrades.Length; a++)
        {
            availableUpgrades.Add(abilityThreeUpgrades[a].upgrade[0]);
        }
        OnAwake.Invoke();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(first)
        {
            OnEnableMenu.Invoke();
            SelectRandomUpgrades();
        } else {
            first = true;
        }
    }

    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SelectRandomUpgrades();
        }
    }
    */

    private void SelectRandomUpgrades()
    {
        ClearPreviousUpgrades();
        int currentRandomNumber = 0;
        if(availableUpgrades.Count == 0) return;
        if(availableUpgrades.Count >= upgradeName.Length)
        {
            currentRandoms = new int[upgradeName.Length];
            upgradeSelection = new Upgrade[upgradeName.Length];
        } else {
            currentRandoms = new int[availableUpgrades.Count];
            upgradeSelection = new Upgrade[availableUpgrades.Count];
        }

        for(int a = 0; a < 1000; a++)
        {
            currentRandoms[currentRandomNumber] = Random.Range(0, availableUpgrades.Count);
            for(int b = 0; b < currentRandomNumber; b++)
            {
                if(currentRandoms[currentRandomNumber] == currentRandoms[b])
                {
                    currentRandomNumber--;
                    break;
                }
            }
            currentRandomNumber++;
            if(currentRandomNumber == currentRandoms.Length)
            {
                break;
            }
        }

        for(int a = 0; a < upgradeSelection.Length; a++)
        {
            upgradeSelection[a] = availableUpgrades[currentRandoms[a]];
            DisplayUpgrade(a);
        }
    }

    private void ClearPreviousUpgrades()
    {
        for(int a = 0; a < upgradeName.Length; a++)
        {
            upgradeButton[a].interactable = false;
            upgradeName[a].text = "";
            upgradeDescription[a].text = "";
            upgradeIcon[a].sprite = null;
        }
    }

    private void DisplayUpgrade(int upgradeNumber)
    {
        upgradeButton[upgradeNumber].interactable = true;
        upgradeName[upgradeNumber].text = upgradeSelection[upgradeNumber].upgradeName;
        upgradeDescription[upgradeNumber].text = upgradeSelection[upgradeNumber].description;
        upgradeIcon[upgradeNumber].sprite = upgradeSelection[upgradeNumber].icon;
    }

    public void UpgradeAbility(int selectedNumber)
    {
        Upgrade currentUpgrade = upgradeSelection[selectedNumber];
        availableUpgrades.Remove(currentUpgrade);

        switch(currentUpgrade.abilityNumber)
        {
            case(0):
                grenade.upgradePaths[currentUpgrade.pathNumber] = currentUpgrade.upgrade;
                for(int a = 0; a < currentUpgrade.linkedUpgrades.Length; a++)
                {
                    if(abilityOneUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Add(abilityOneUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]]);
                    }
                }

                for(int a = 0; a < currentUpgrade.lockedUpgrades.Length; a++)
                {
                    if(abilityOneUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Remove(abilityOneUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]]);
                    }
                }
            break;

            case(1):
                magneticCore.upgradePaths[currentUpgrade.pathNumber] = currentUpgrade.upgrade;
                for(int a = 0; a < currentUpgrade.linkedUpgrades.Length; a++)
                {
                    if(abilityTwoUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Add(abilityTwoUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]]);
                    }
                }

                for(int a = 0; a < currentUpgrade.lockedUpgrades.Length; a++)
                {
                    if(abilityTwoUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Remove(abilityTwoUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]]);
                    }
                }
            break;

            case(2):
                combatStim.upgradePaths[currentUpgrade.pathNumber] = currentUpgrade.upgrade;
                for(int a = 0; a < currentUpgrade.linkedUpgrades.Length; a++)
                {
                    if(abilityThreeUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Add(abilityThreeUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.linkedUpgrades[a]]);
                    }
                }

                for(int a = 0; a < currentUpgrade.lockedUpgrades.Length; a++)
                {
                    if(abilityThreeUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]] is not null)
                    {
                        availableUpgrades.Remove(abilityThreeUpgrades[currentUpgrade.pathNumber].upgrade[currentUpgrade.lockedUpgrades[a]]);
                    }
                }
            break;
        }

        currentUpgrade.lineGenerator.GenerateLine(currentUpgrade.lineNumber);
        OnUpgrade.Invoke();
    }
}


[System.Serializable]
public class UpgradePath
{
    public Upgrade[] upgrade;
}
