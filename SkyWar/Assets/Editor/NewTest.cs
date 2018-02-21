using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewTest {

	[UnityTest]
	public IEnumerator NewTestWithEnumeratorPasses()
	{
        var enemySpawner = new GameObject().AddComponent<EnemyWaveSpawner>();

	    var enemyPrefab = GameObject.FindWithTag("Enemy");
	    var spwanedEnemy = PrefabUtility.GetPrefabParent(enemyPrefab);

		yield return null;

        Assert.AreEqual(enemyPrefab, spwanedEnemy);
	}

    public IEnumerator TestPlayerStatsEnumerator()
    {
        var playerStats = new GameObject().AddComponent<PlayerStats>();

        playerStats.StartingMoney = 2000f;

        yield return null;

        Assert.AreEqual(playerStats.StartingMoney, 2000f);
    }

    [TearDown]
    public void AfterEveryTest()
    {
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
            GameObject.DestroyImmediate(gameObject);

        foreach (var gameObject in Object.FindObjectsOfType<EnemyWaveSpawner>())
            Object.DestroyImmediate(gameObject);
    }
}
