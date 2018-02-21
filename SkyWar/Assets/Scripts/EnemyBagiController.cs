using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyBagiController : MonoBehaviour
{
    // The prefab bullet which the enemy spawns to hit a target.
    public GameObject BulletPrefab;
    // Audio clip when firing.
    public AudioClip FireSound;

    // The object responsible for movement.
    private NavMeshAgent agent;
    // The closest target.
    private Transform target;
    // The closest firing target.
    private Transform firingTarget;

    [Header("Enemy Specs")]
    public float DetectionRange = 500f;
    public float FiringRange = 30f;
    public float Health;
    public float Worth;
    public float FireRate = 2f;
    public float FireCooldown = 1f;
    
    // Starting specs for enemies, same for everyone.
    private float _startingHealth = 100f;
    private float _startingWorth = 40f;

    // The Tag that decides which objects are to be attacked
    private string tag = "Player";

    // Initializing specs and invoking the "UpdateTarget"-method.
    // Explained later.
    void Start()
    {
        Health = _startingHealth;
        Worth = _startingWorth;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = FiringRange;
        InvokeRepeating("UpdateTarget", 0f, 0.33f);
    }

    // Checks for a target, if it finds one, it will move to its position.
    // If the enemy is close enough, it will fire at its target.
    void Update()
    {
        if (target != null)
        {
            MoveToPosition(target.transform.position);
        }

        if (firingTarget != null)
        {
            Vector3 direction = transform.position - target.position;
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

    // Called every half second to update the target.
    // To know if it was destroyed or something like that.
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        float shortestDistance = Mathf.Infinity;
        float shortestFiringDistance = Mathf.Infinity;
        GameObject nearestFiringEnemy = null;
        GameObject nearestEnemy = null;

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

        if (nearestEnemy != null && shortestDistance <= DetectionRange)
        {
            target = nearestEnemy.transform;
        }
        else target = null;

        if (nearestFiringEnemy != null && shortestFiringDistance <= FiringRange)
        {
            firingTarget = nearestFiringEnemy.transform;
        }

    }

    // Instatinate bullet and set its target.
    // Plays firing audio clip.
    void Shoot()
    {
        GameObject bulletObject = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(FireSound, transform.position);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    // When the enemy is hit.
    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
        EnemyWaveSpawner.EnemiesAlive--;
        PlayerStats.Money += Worth;
    }

    // Upgrading health.
    public void IncreaseHealth(float amount)
    {
        _startingHealth *= amount;
    }

    // Upgrading Worth.
    public void IncreaseWorth(float amount)
    {
        _startingWorth *= amount;
    }

    public void SetAttributes()
    {
        Health = _startingHealth;
        Worth = _startingWorth;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FiringRange);
    }
}
