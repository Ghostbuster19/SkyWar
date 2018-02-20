using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{

    [Header("Attributes")]
    public AudioClip Explosion;
    public AudioClip DestructionClip;

    public static float StartingHealth;
    public float Health;
    public float Range = 10f;
    public float FireRate = 1f;
    public float RotationSpeed = 10f;
    public float ResetSpeed = 10f;
    public GameObject RocketPrefab;
    public Transform FirePoint;
    public  string Tag = "Enemy";

    private Transform target;
    private Quaternion startingPosition;
    private float fireCooldown = 0f;

    // Use this for initialization
    void Start()
    {
        StartingHealth = Health;
        Debug.Log("Satrting health: " + StartingHealth);
        startingPosition = transform.rotation;
        InvokeRepeating("UpdateTarget", 0f, 0.33f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
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

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startingPosition, Time.deltaTime * ResetSpeed);
            return;
        }
            

        Vector3 direction = transform.position - target.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / FireRate;
        }

        fireCooldown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject rocketObject = Instantiate(RocketPrefab, FirePoint.position, FirePoint.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(Explosion, FirePoint.position);
        Rocket rocket = rocketObject.GetComponent<Rocket>();

        if (rocket != null)
        {
            rocket.Seek(target);
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            AudioSource.PlayClipAtPoint(DestructionClip, transform.position);
            Destroy(gameObject);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
