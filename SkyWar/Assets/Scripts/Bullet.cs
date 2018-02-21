using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Bullet stats.
    public float Speed = 10f;
    public static float StartingDamage = 20f;
    public float Damage;

    // The target to follow.
    private Transform _target;

    // Setting the damage of the bullet.
    void Start()
    {
        Damage = StartingDamage;
    }

    // Increasing damage when upgraded.
    public static void IncreaseDamage(float amount)
    {
        StartingDamage *= amount;
    }

    // Set the target of this bullet.
    public void Seek(Transform target)
    {
        _target = target;
    }

    // If the bullet has no target, target already destroyed, 
    // then it will be destroyed.
    // Instead it will follow the target.
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

    // If the bullet is in impact distance it damages the target
    // and destroys itself.
    void HitTarget()
    {
        DamageTarget(_target);
        Destroy(gameObject);
    }

    // Decides what target to damage since it can be a Bagi or a turret.
    void DamageTarget(Transform thisTarget)
    {
        EnemyBagiController enemy = thisTarget.GetComponent<EnemyBagiController>();
        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
        else
        {
            EnemyTurretController turretController = thisTarget.GetComponent<EnemyTurretController>();
            turretController.TakeDamage(Damage);
        }
    }
}
