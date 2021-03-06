﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    // Enum to distinguish between big and small turrets.
    // Can contain more options if another type is to be added.
    public enum Type
    {
        BIG,
        SMALL
    }
    // Audio clip when firing.
    public AudioClip Explosion;
    // Audio clip when destroyed.
    public AudioClip DestructionClip;
    // The current target.
    private Transform _target;
    // The Rocket which will be spawned.
    public GameObject RocketPrefab;
    // The position from which the rocket will be spawned.
    public Transform FirePoint;
    // Choose between the big base turrets or the outter normal turrets
    public Type type;

    // The starting rotation of the turret.
    private Quaternion _startingPosition;

    [Header("Attributes")]
    public static float UpgradeCost = 5000f;
    public float StartingHealth;
    public float Health;
    public float Range = 50f;
    public float FireRate = 1f;
    public static float RotationSpeed = 10f;
    public static float ResetSpeed = 10f;
    
    public  string Tag = "EnemyBagi";
    
    private float _fireCooldown = 0f;

    // Invokes the "UpdateTarget"-Method every half second to look for enemies.
    void Start()
    {
        if (type == Type.BIG)
        {
            Health = 5000;
            FireRate = 5;
            Range = 100;
        }
        else
        {
            Health = 2000;
            FireRate = 2;
            Range = 50;
        }

        StartingHealth = Health;
        _startingPosition = transform.rotation;
        InvokeRepeating("UpdateTarget", 0f, 0.33f);
    }

    // Looks for the closest enemy and sets it as its target.
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
            _target = nearestEnemy.transform;
        }
        else _target = null;

    }

    // Check if enemy is still alive.
    // Shoot the enemy.
    void Update()
    {
        if (_target == null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _startingPosition, Time.deltaTime * ResetSpeed);
            return;
        }
            
        Vector3 direction = transform.position - _target.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (_fireCooldown <= 0f)
        {
            Shoot();
            _fireCooldown = 1f / FireRate;
        }

        _fireCooldown -= Time.deltaTime;
    }

    // Instantiating rocket and setting its target.
    void Shoot()
    {
        GameObject rocketObject = Instantiate(RocketPrefab, FirePoint.position, FirePoint.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(Explosion, FirePoint.position);
        Rocket rocket = rocketObject.GetComponent<Rocket>();

        if (rocket != null)
        {
            rocket.Seek(_target);
        }
    }

    // Receive damage when hit.
    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            AudioSource.PlayClipAtPoint(DestructionClip, transform.position);
            Destroy(gameObject);
        }
    }

    // Method for reparing the turret.
    public void ResetHealth()
    {
        Health = StartingHealth;
    }

    public void UpgradeTurretStats(float amount)
    {
        Range *= amount;
        // If the turret is not damaged then increase its current health as well.
        if (Health == StartingHealth)
            Health *= amount;
        StartingHealth *= amount;
    }

    // In editor only method which displays the range of the tower.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}