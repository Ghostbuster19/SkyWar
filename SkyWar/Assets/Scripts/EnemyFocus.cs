using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocus : MonoBehaviour
{

    public float Range = 100f;
    public float RotationSpeed = 10f;

    private Transform parentTransform;
    private Transform target;
    private Quaternion startingRotation;
    private string tag = "Enemy";
    
    // Use this for initialization
    void Start ()
    {
        startingRotation = transform.rotation;
        parentTransform = gameObject.GetComponentInParent<Transform>();
        InvokeRepeating("FocusEnemy", 0f, 0.33f);
	}

    void FocusEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            
            float distanceToEnemy = Vector3.Distance(parentTransform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= Range)
        {
            target = nearestEnemy.transform;
        }
        else target = null;
    }

    void Update()
    {
        if (target == null)
        {
            transform.rotation = startingRotation;
            return;
        }

        Vector3 distance = target.transform.position - transform.position;
        
    }
}
