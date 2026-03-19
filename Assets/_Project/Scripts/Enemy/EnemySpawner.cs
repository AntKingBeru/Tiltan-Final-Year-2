using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemy(Core core)
    {
        var spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        var enemy = Instantiate(
            enemyPrefab,
            spawn.position,
            Quaternion.identity
        );

        enemy.Initialize(core);
    }
}