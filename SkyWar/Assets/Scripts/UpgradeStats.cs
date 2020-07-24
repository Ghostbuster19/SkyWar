using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats : MonoBehaviour {

    // Audio clip when there are insufficient funds for upgrading.
    public AudioClip UpgradeError;
    // Audio clip when upgarde is successful.
    public AudioClip UpgradeSuccess;

    // How many upgardes have already been done.
    private int NumberOfUpgrades = 0;
    private const int MaxNumberOfUpgrades = 5;

    // Tag to distinguish the player turrets
    public string Tag = "Player";

    public void UpgradeStatsMethod(float amount)
    {
        if (PlayerStats.Money < PlayerTurretController.UpgradeCost || NumberOfUpgrades >= MaxNumberOfUpgrades)
        {
            AudioSource.PlayClipAtPoint(UpgradeError, transform.position);
            return;
        }

        GameObject[] objects = GameObject.FindGameObjectsWithTag(Tag);
        if (objects != null)
        {
            foreach (GameObject go in objects)
            {
                PlayerTurretController controller = go.GetComponent<PlayerTurretController>();
                if (controller != null)
                {
                    controller.UpgradeTurretStats(amount);
                }
            }
        }

        PlayerStats.Money -= PlayerTurretController.UpgradeCost;
        AudioSource.PlayClipAtPoint(UpgradeSuccess, Camera.main.transform.position);        
    }
}
