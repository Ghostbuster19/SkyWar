using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{

    // The Bagi prefab which will be spawned.
    public GameObject UnitPrefab;
    // The point at which the Bagi will be spawned.
    public Transform SpawnPoint;

    private BagiController controller;

    void Start()
    {
        controller = UnitPrefab.GetComponent<BagiController>();
    }

    // Method called when creating a new Bagi through the buy button.
    public void SpawnUnit()
    {
        if (PlayerStats.Money < controller.Cost)
            return;
        Instantiate(UnitPrefab, SpawnPoint.position, SpawnPoint.rotation);
        PlayerStats.Money -= controller.Cost;
    }
}
