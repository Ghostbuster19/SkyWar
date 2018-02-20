using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{

    public static int EnemiesAlive;
    public Wave[] Waves;
    public Transform SpwanPoint;
    public float TimeBetweenWaves = 30f;
    private const float TimeForTheBigOne = 120f;
    public float BigOneCooldown = 120f;
    public float Cooldown = 2f;
    public bool PowerUp = true;

    private int waveIndex = 0;


	// Update is called once per frame
	void Update () {

	    BigOneCooldown -= Time.deltaTime;

        if (EnemiesAlive > 0)
	    {
	        return;
	    }

	    if (waveIndex == Waves.Length)
	    {
	        waveIndex = Waves.Length - 1;
	        PowerUp = false;
	    }

	    if (BigOneCooldown <= 0)
	    {
	        BigOneCooldown = TimeForTheBigOne;

	        SpawnTheBigOne();

	        return;
	    }

	    if (Cooldown <= 0)
	    {
	        StartCoroutine(SpawnWave());
	        Cooldown = TimeBetweenWaves;
	        BigOneCooldown -= Time.deltaTime;
	        return;
        }

	    

        Cooldown -= Time.deltaTime;
	    Cooldown = Mathf.Clamp(Cooldown, 0f, Mathf.Infinity);
    }

    IEnumerator SpawnWave()
    {
        Wave wave = Waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        GameObject go = Instantiate(enemy, SpwanPoint.position, SpwanPoint.rotation) as GameObject;

        EnemyBagiController controller = go.GetComponent<EnemyBagiController>();

        if (PowerUp)
        {
            EnemyRocket.IncreaseDamage(1.25f);
            controller.IncreaseHealth(1.5f);
            controller.IncreaseWorth(1.2f);
        }
    }

    void SpawnTheBigOne()
    {
        // TODO: Spawn the BIG ONE
    }
}
