using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public float Speed = 10f;
    public static float StartingDamage = 50f;
    public float Damage;
    private Transform _target;

    void Start()
    {
        Damage = StartingDamage;
    }

    public static void IncreaseDamage(float amount)
    {
        StartingDamage *= amount;
    }

	// Use this for initialization
    public void Seek(Transform target)
    {
        _target = target;
    }
	
	// Update is called once per frame
	void Update () {
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

    void HitTarget()
    {
        DamageTarget(_target);
        Destroy(gameObject);
    }

    void DamageTarget(Transform thisTarget)
    {
        EnemyBagiController enemy = thisTarget.GetComponent<EnemyBagiController>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
        else Debug.Log("Didnt find Enemy");
    }
}
