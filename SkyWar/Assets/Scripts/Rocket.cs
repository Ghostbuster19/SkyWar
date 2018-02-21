using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Rocket stats.
    public float Speed = 10f;
    public static float StartingDamage = 50f;
    public float Damage;

    // The rockets target.
    private Transform _target;

    void Start()
    {
        Damage = StartingDamage;
    }

    // Increase Damage when upgraded.
    public static void IncreaseDamage(float amount)
    {
        StartingDamage *= amount;
    }

	// Set the rockets target.
    public void Seek(Transform target)
    {
        _target = target;
    }
	
	// Check rockets target if it still exists.
    // Damage target when in reaching distance.
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

    // Damage target, destroy rocket.
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
