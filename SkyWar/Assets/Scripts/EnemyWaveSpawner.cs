using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public Wave[] Waves;
    public Transform SpwanPoint;

    // The number of enemies still alive in this wave.
    public static int EnemiesAlive;
    
    public float TimeBetweenWaves = 30f;
    public float Cooldown = 2f;
    public bool PowerUp = false;

    // Which wave is to spawn.
    private int waveIndex = 0;

	// If there still enemies alive, it waits for them ti die.
    // Then it decides which wave to spawn.
    // All that while decresaing the time till the next wave.
	void Update () {

        if (EnemiesAlive > 0)
	    {
	        return;
	    }

	    if (waveIndex == Waves.Length)
	    {
	        waveIndex = Waves.Length - 1;
	        PowerUp = false;
	    }

	    if (Cooldown <= 0)
	    {
	        StartCoroutine(SpawnWave());
	        Cooldown = TimeBetweenWaves;
	        return;
        }

        Cooldown -= Time.deltaTime;
	    Cooldown = Mathf.Clamp(Cooldown, 0f, Mathf.Infinity);
    }

    // Method for spawning the enemies.
    // Runs in a coroutine.
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

    // Mehtod for spawning each enemy.
    void SpawnEnemy(GameObject enemy)
    {
        GameObject go = Instantiate(enemy, SpwanPoint.position, SpwanPoint.rotation) as GameObject;

        if (PowerUp)
        {
            EnemyBagiController controller = go.GetComponent<EnemyBagiController>();
            EnemyRocket.IncreaseDamage(1.25f);
            controller.IncreaseHealth(1.5f);
            controller.IncreaseWorth(1.2f);
        }
    }
}
