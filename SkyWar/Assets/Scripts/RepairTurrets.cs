using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTurrets : MonoBehaviour
{

    private const float RepairCost = 2000f;
    public string Tag = "Player";

    // Check if player has enough money to repair his turrets.
    // Reset every turrets health back to full.
    public void Repair()
    {
        if (PlayerStats.Money < RepairCost)
        {
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
                    controller.ResetHealth();
                }
            }

            PlayerStats.Money -= RepairCost;
        }
    }

}
