using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsController : MonoBehaviour {

    [Header("Combat")]
    public int zombieKills;
    public int totalZombiesSpawned;
    public int zombiesAttacked;
    public int timesPlayerGotAttacked;
    public int weaponsCollected;

    [Header("General")]
    public int levelReached;
    public int foodFound;
    public int timesTimerReachedZero;

    //Weapons
    int standardWeaponUsed;
    int meleeWeaponUsed;
    int shotgunWeaponUsed;
    int trapsPlaced;
    int totalWeaponsUsed;

    [Header("Crates")]
    public int spawnedCrates;
    public int collectedCrates;

    public void WeaponUsed(string weaponName)
    {
        totalWeaponsUsed++;

        switch (weaponName)
        {
            case "Shotgun":
                shotgunWeaponUsed++;
                break;

            case "Traps":
                trapsPlaced++;
                break;

            case "Melee Weapon":
                meleeWeaponUsed++;
                break;

            case "Standard":
                standardWeaponUsed++;
                break;

            default:
                //Do Nothing
                standardWeaponUsed++;
                break;
        }
    }

    public void StartAnalytics()
    {
        SendWeaponsUsed();
        GeneralInfo();
        //Debug.Log("Send Analytics!");
    }

    void SendWeaponsUsed()
    {
        Analytics.CustomEvent("WeaponStats", new Dictionary<string, object>
        {
            { "standardWeaponUsed", standardWeaponUsed },
            { "meleeWeaponUsed", meleeWeaponUsed },
            { "shotgunWeaponUsed", shotgunWeaponUsed },
            { "trapsPlaced", trapsPlaced },
            { "totalWeaponsUsed", totalWeaponsUsed },

            { "weaponsCollected", weaponsCollected }
        });

        Analytics.CustomEvent("Crates", new Dictionary<string, object>
        {
            { "spawnedCrates", spawnedCrates },
            { "collectedCrates", collectedCrates },

        });
    }

    void GeneralInfo()
    {
        Analytics.CustomEvent("GeneralInfo", new Dictionary<string, object>
        {
            { "zombieKills", zombieKills },
            { "totalZombiesSpawned", totalZombiesSpawned },
            { "zombiesAttacked", zombiesAttacked },
            { "timesPlayerGotAttacked", timesPlayerGotAttacked },
            { "levelReached", levelReached },
            { "foodFound", foodFound },
            { "timesTimerReachedZero", timesTimerReachedZero }
        });
    }

    public void Reset()
    {
        zombieKills = 0;
        weaponsCollected = 0;
        levelReached = 0;
        foodFound = 0;
        timesTimerReachedZero = 0;

        weaponsCollected = 0;
        standardWeaponUsed = 0;
        meleeWeaponUsed = 0;
        shotgunWeaponUsed = 0;
        trapsPlaced = 0;

        collectedCrates = 0;
        spawnedCrates = 0;

        Debug.Log("Reset!");
    }
}
