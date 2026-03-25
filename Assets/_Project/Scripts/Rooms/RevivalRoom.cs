using UnityEngine;

public class RevivalRoom : Room
{
    [Header("Setup")]
    [SerializeField] private Transform corpseAnchor;
    [SerializeField] private Transform spawnPoint;
    
    private GameObject _corpse;
    
    public bool HasCorpse => _corpse;
    
    #region Interaction

    public bool CanInteract(PlayerController player)
    {
        if (!player)
            return false;
        
        return (player.HasCorpse && !HasCorpse) || HasCorpse;
    }
    
    public void Interact(PlayerController player)
    {
        if (!player || !player.HasCorpse || HasCorpse)
        {
            var corpse = player.DropCorpse();
            
            if (!corpse)
                return;
            
            TryPlaceCorpse(corpse.gameObject);
            return;
        }

        if (HasCorpse)
        {
            Revive();
        }
    }
    
    #endregion
    
    #region Public API
    
    public bool TryPlaceCorpse(GameObject corpse)
    {
        if (HasCorpse)
            return false;
        
        _corpse = corpse;
        
        corpse.transform.position = corpseAnchor.position;
        corpse.transform.rotation = corpseAnchor.rotation;

        if (corpse.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;
        
        return true;
    }
    
    #endregion
    
    #region Spawning

    private void Revive()
    {
        var spawnSkeleton = Random.value < 0.5f;
        
        if (spawnSkeleton)
            SpawnSkeleton();
        else
            SpawnZombie();
        
        ConsumeCorpse();
    }
    
    private void SpawnSkeleton()
    {
        MinionManager.Instance.SpawnRevivalMinion(spawnPoint.position, true);
    }
    
    private void SpawnZombie()
    {
        MinionManager.Instance.SpawnRevivalMinion(spawnPoint.position, false);
    }
    
    private void ConsumeCorpse()
    {
        if (_corpse)
            Destroy(_corpse);
        
        _corpse = null;
    }
    
    #endregion
}