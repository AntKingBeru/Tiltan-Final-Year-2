using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    
    [SerializeField] private EnemySpawner spawner;
    
    [Header("Wave Settings")]
    [SerializeField] private int totalWaves = 10;
    [SerializeField] private float timeBetweenWaves = 10f;

    [SerializeField] private int baseEnemyCount = 3;

    private int _currentWave;
    
    public int CurrentWave => _currentWave;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(3f);

        while (_currentWave < totalWaves)
        {
            _currentWave++;

            SpawnWave(_currentWave);
            
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnWave(int wave)
    {
        var enemyCount = baseEnemyCount + wave * 2;
        
        for (var i = 0; i < enemyCount; i++)
            spawner.SpawnEnemy();
    }
}