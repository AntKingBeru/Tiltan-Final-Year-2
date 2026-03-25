using UnityEngine;

public class BarracksSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnCooldown = 2f;
    
    private float _lastSpawnTime = -Mathf.Infinity;
    
    public bool CanInteract(PlayerController player)
    {
        return Time.time - _lastSpawnTime < spawnCooldown ? false : MinionManager.Instance;
    }

    public void Interact(PlayerController player)
    {
        if (!CanInteract(player))
            return;
        
        MinionManager.Instance.SpawnMinion(spawnPoint.position);
        _lastSpawnTime = Time.time;
    }
}