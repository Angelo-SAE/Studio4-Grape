using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    //Upgrade Order : A AA AAA AAB AB ABA ABB
    //Upgrade Number : 0 1   2   3  4   5   6

    //Ability Numbers
    //Grenade - 0
    //MagneticCore - 1
    //CombatStim  - 2

    [Header("Upgrade Variables")]
    public int abilityNumber; //number for ability
    public int upgradeNumber; //number for which upgrade
    public int[] linkedUpgrades; //numbers for linked upgrades (upgrade A leads to upgrades AA and AB. So upgrade A linked upgrades equals 1 and 4)
    public int[] lockedUpgrades; //numbers for locked upgrades (upgrade AA locks of upgrade AB so this should equal 4)
    public int pathNumber; //number for which upgrade path
    public AbilityUpgradePath.Upgrade upgrade;
    public Sprite icon;
    public string upgradeName;
    public string description;
    public UpgradeLineGenerator lineGenerator;
    public int lineNumber;
    //public bool acquired;
}
