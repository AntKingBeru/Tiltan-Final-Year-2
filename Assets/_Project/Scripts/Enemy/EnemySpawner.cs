using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemy(Core core)
    {
        var pos = GetSpawnPosition();

        var enemy = Instantiate(
            enemyPrefab,
            pos,
            Quaternion.identity
        );

        enemy.Initialize(core);
    }

    private Vector3 GetSpawnPosition()
    {
        var cells = GridManager.Instance.GetValidEnemyEntryCells();
        
        if (cells.Count == 0)
            return Vector3.zero;
        
        var chosen = cells[Random.Range(0, cells.Count)];
        
        var worldPos = GridManager.Instance.GridToWorld(chosen);
        
        // Offset slightly OUTSIDE dungeon
        return worldPos + (worldPos - transform.position).normalized * 5f;
    }
}