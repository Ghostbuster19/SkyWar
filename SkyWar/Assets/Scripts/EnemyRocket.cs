using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : MonoBehaviour {

    // Rocket Specs.
    public float Speed = 10f;
    public static float StartingDamage = 50f;
    public float Damage;

    // The rockets target.
    private Transform _target;

    // Set the rockets damage.
    void Start()
    {
        Damage = StartingDamage;
    }

    // Called when upgrading.
    public static void IncreaseDamage(float amount)
    {
        StartingDamage *= amount;
    }

    // Set the rockets target.
    public void Seek(Transform target)
    {
        _target = target;
    }

    // Check if rocket is in damaging distance.
    // If yes, damage enemy and destroy rocket.
    // if not, keep moving towards target.
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

        transform.LookAt(_target);
        transform.Translate(distance.normalized * distanceThisFrame, Space.World);
    }

    // Damage the target and destroy rocket.
    void HitTarget()
    {
        DamageTarget(_target);
        Destroy(gameObject);
    }

    void DamageTarget(Transform thisTarget)
    {
        BagiController enemy = thisTarget.GetComponent<BagiController>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
    }
}
