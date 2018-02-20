using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(LayerMask))]
public class BagiController : MonoBehaviour
{

    public LayerMask layer;
    public bool selected = false;
    public static float StartingHealth = 100f;
    public float Health;
    public static float StartingCost = 100f;
    public float Cost;
    public float FireRate = 2f;
    public float FireCooldown = 1f;

 
    public GameObject BulletPrefab;
    public AudioClip FireSound;

// The number which indicates the right mouse button
    private const int RIGHT_MOUSE_BUTTON = 1;
    private NavMeshAgent agent;
    private Transform target;
    private GameObject firingTarget;

    private bool isDead = false;

    // Use this for initialization
    void Start ()
    {
        Health = StartingHealth;
        Cost = StartingCost;
	    agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON))
	    {
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hit;
           
	        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer) && selected)
            {
                MoveToPosition(hit.point);
	        }
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

    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    void Shoot()
    {
        GameObject bulletObject = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(FireSound, transform.position);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
            Die();
    }

    public static void IncreaseHealth(float amount)
    {
        StartingHealth += amount;
    }

    public static void IncreaseCost(float amount)
    {
        StartingCost += amount;
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
