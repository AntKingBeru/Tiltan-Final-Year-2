using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Core core;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 6f;
    [SerializeField] private float navMeshSampleRadius = 3f;
    [SerializeField] private int maxAttempts = 10;

    public void SpawnEnemy()
    {
        var spawnPos = GetSpawnPosition();

        if (spawnPos == Vector3.zero)
            return;

        var enemy = Instantiate(
            enemyPrefab,
            spawnPos,
            Quaternion.identity
        );

        enemy.Initialize(core);
    }

    private Vector3 GetSpawnPosition()
    {
        var cells = GridManager.Instance.GetValidEnemyEntryCells();
        
        if (cells.Count == 0)
            return Vector3.zero;

        for (var i = 0; i < maxAttempts; i++)
        {
            var chosen = cells[Random.Range(0, cells.Count)];

            var cellWorld = GridManager.Instance.GridToWorld(chosen);

            // Direction from the dungeon's center outwards
            var dungeonCenter = GetDungeonCenter();
            var dir = (cellWorld - dungeonCenter).normalized;
            
            var rawSpawn = cellWorld + dir * spawnDistance;
            
            // Snap to NavMesh
            if (NavMesh.SamplePosition(
                    rawSpawn,
                    out var hit,
                    navMeshSampleRadius,
                    NavMesh.AllAreas))
            {
                // Ensure the path exists to the core
                if (HasPathToCore(hit.position))
                    return hit.position;
            }
        }
        
        return Vector3.zero;
    }

    private Vector3 GetDungeonCenter()
    {
        return GridManager.Instance.GridToWorld(new Vector2Int(
            GridManager.Instance.Width / 2,
            GridManager.Instance.Height / 2
        ));
    }

    private bool HasPathToCore(Vector3 spawnPos)
    {
        var path = new NavMeshPath();

        if (NavMesh.CalculatePath(
                spawnPos,
                core.transform.position,
                NavMesh.AllAreas,
                path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        
        return false;
    }
}