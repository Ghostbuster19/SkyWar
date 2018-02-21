using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats : MonoBehaviour {

    // Audio clip when there are insufficient finds for upgrading.
    public AudioClip UpgradeError;
    // Audio clip when upgarde is successful.
    public AudioClip UpgradeSuccess;

    // How many upgardes have already been done.
    private int NumberOfUpgrades = 0;
    private const int MaxNumberOfUpgrades = 5;

    public void UpgradeStatsMethod(float amount)
    {
        if (PlayerStats.Money < PlayerTurretController.UpgradeCost || NumberOfUpgrades >= MaxNumberOfUpgrades)
        {
            AudioSource.PlayClipAtPoint(UpgradeError, transform.position);
            return;
        }
        // Increase range and health of the turrets.
        PlayerTurretController.Range *= amount;
        PlayerTurretController.StartingHealth *= amount;
        PlayerStats.Money -= PlayerTurretController.UpgradeCost;
        AudioSource.PlayClipAtPoint(UpgradeSuccess, Camera.main.transform.position);
    }
}
