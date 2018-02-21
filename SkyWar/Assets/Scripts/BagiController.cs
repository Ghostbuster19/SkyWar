using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(LayerMask))]
public class BagiController : MonoBehaviour
{
    // The object that will be spawned as the bullet.
    public GameObject BulletPrefab;
    // An audio clip that runs when a bullet is fired.
    public AudioClip FireSound;

    // The starting health and cost of each Bagi.
    // Made static to be the same in every Bagi-object.
    public static float StartingHealth = 100f;
    public static float StartingCost = 100f;

    // Bagi stats
    public float Health;
    public float Cost;
    public float FireRate = 2f;
    public float FireCooldown = 1f;
    // The detection range
    public float Range = 500f;
    // The range from which a Bagi will start shooting.
    public float FiringRange = 30f;

    // This tag indicates the what objects to look for when deciding the target.
    public string Tag = "EnemyBagi";

    // The object responsible for moving the Bagi on the map;
    private NavMeshAgent _agent;
    // Target to move towards, assigned automaticly
    private Transform _target;
    // Closest target within the Range, so units can auto attack
    private Transform _firingTarget;

    // Initalize health, Cost and moving component.
    // Starts the "UpdateFiringTarget"-method every half second.
    // The method is explained later on.
    void Start ()
    {
        Health = StartingHealth;
        Cost = StartingCost;
	    _agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("UpdateFiringTarget", 0f, 0.5f);
    }
	
    // If the Bagi has a target, it will move to its position,
    // and if it is in its firing range it will shoot at it.
	void Update ()
    {

	    if (_target != null)
	    {
	        MoveToPosition(_target.transform.position);
	    }

        if (_firingTarget != null)
        {
            Vector3 direction = transform.position - _target.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (FireCooldown <= 0f)
            {
                Shoot();
                FireCooldown = 1f / FireRate;
            }

            FireCooldown -= Time.deltaTime;
        }
    }

    // This method looks for all objects with the above mentioned tag,
    // and decides which one is the closest.
    void UpdateFiringTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag);
        float shortestDistance = Mathf.Infinity;
        float shortestFiringDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject nearestFiringEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            if (distanceToEnemy < shortestFiringDistance)
            {
                shortestFiringDistance = distanceToEnemy;
                nearestFiringEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= Range)
        {
            _target = nearestEnemy.transform;
        }

        if (nearestFiringEnemy != null && shortestDistance <= FiringRange)
        {
            _firingTarget = nearestEnemy.transform;
        }
        else _firingTarget = null;
    }

    // Makes the Bagi move to its target
    public void MoveToPosition(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    // Instaiates a bullet and gives it the target.
    // The bullet has a script to follow and damage the target.
    void Shoot()
    {
        GameObject bulletObject = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(FireSound, transform.position);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(_firingTarget);
        }
    }

    // If the Bagi itself is damaged, this mehtod will 
    // reduce its health.
    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
            Die();
    }

    // Method for upgrading the health.
    public static void IncreaseHealth(float amount)
    {
        StartingHealth += amount;
    }

    // Method for upgrading the cost.
    public static void IncreaseCost(float amount)
    {
        StartingCost *= amount;
    }

    // Called when the Bagis health is 0.
    void Die()
    {
        Destroy(gameObject);
    }

    // This method displays the range of the Bagi in the
    // Editor only.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
