using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyBagiController : MonoBehaviour
{
    public float DetectionRange = 500f;
    public AudioClip FireSound;

    [Header("Enemy Specs")]
    public float FiringRange = 30;
    public float Health;
    public float Worth;
    public float FireRate = 2f;
    public float FireCooldown = 1f;
    public GameObject BulletPrefab;

    private float _startingHealth = 100f;
    private float _startingWorth = 40f;

    // The number which indicates the right mouse button
    private const int RIGHT_MOUSE_BUTTON = 1;
    
    private NavMeshAgent agent;
    private Transform target;
    private Transform firingTarget;

    // The Tag that decides which objects are to be attacked
    private string tag = "Player";

    private bool isDead;

    // Use this for initialization
    void Start()
    {
        Health = _startingHealth;
        Worth = _startingWorth;
        isDead = false;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = FiringRange;
        InvokeRepeating("UpdateTarget", 0f, 0.33f);
    }

    // Update is called once per frame
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

    public void TakeDamage(float amount)
    {
        Debug.Log("Received Damage: " + amount + "Decreasing Health from: " + Health);
        Health -= amount;
        Debug.Log("Decresed Heath to: " + Health);
        if (Health <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
        EnemyWaveSpawner.EnemiesAlive--;
    }

    public void IncreaseHealth(float amount)
    {
        _startingHealth *= amount;
    }

    public void IncreaseWorth(float amount)
    {
        _startingWorth *= amount;
    }

    public void SetAttributes()
    {
        Debug.Log("Setting Attributes");
        Debug.Log("_startingHealth: " + _startingHealth + ", StartingWort: " + _startingWorth);
        Health = _startingHealth;
        Worth = _startingWorth;
        Debug.Log("Health: " + Health + ", Worth: " + Worth);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FiringRange);
    }
}
