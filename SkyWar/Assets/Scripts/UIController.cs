using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject ToBeSpawned;

    public void SpawnObject()
    {
        Debug.Log("Spawned Vehicle");
        Instantiate(ToBeSpawned, transform.position, transform.rotation, transform);
    }
}
