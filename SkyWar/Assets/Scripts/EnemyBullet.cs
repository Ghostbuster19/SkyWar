using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    // Bullet stats
    public float Speed = 10f;
    public static float StartingDamage = 20f;
    public float Damage;

    // The bullets target.
    public Transform _target;

    void Start()
    {
        Damage = StartingDamage;
    }
    
    public static void IncreaseDamage(float amount)
    {
        StartingDamage *= amount;
    }

    // Set the bullets target.
    public void Seek(Transform target)
    {
        _target = target;
    }
    

    // Check if target is exsisting and move towards it.
    // If the bullet kills the target it will be destroyed.
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 distance = _target.position - transform.position;
        float distanceThisFrame = Speed * Time.deltaTime;

        if (distance.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(distance.normalized * distanceThisFrame, Space.World);
    }

    // Damage the target and destroy bullet.
    void HitTarget()
    {
        if(_target != null)
            DamageTarget(_target);
        Destroy(gameObject);
    }

    // Decide wether the target is a turret or a Bagi.
    void DamageTarget(Transform thisTarget)
    {
        BagiController enemy = thisTarget.GetComponent<BagiController>();
        PlayerTurretController turretController = thisTarget.GetComponent<PlayerTurretController>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
        else if (turretController != null)
        {
            turretController.TakeDamage(Damage);
        }
    }
}
