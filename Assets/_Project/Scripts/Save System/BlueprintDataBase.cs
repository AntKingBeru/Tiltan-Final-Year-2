using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlueprintDataBase : MonoBehaviour
{
    public static BlueprintDataBase Instance { get; private set; }
    
    [Header("Room Blueprints")]
    [SerializeField] private List<RoomBlueprint> roomBlueprints;
    
    [Header("Trap Blueprints")]
    [SerializeField] private List<TrapBlueprint> trapBlueprints;
    
    private Dictionary<string, RoomBlueprint> _roomLookup;
    private Dictionary<string, TrapBlueprint> _trapLookup;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        BuildLookups();
    }

    private void BuildLookups()
    {
        _roomLookup = roomBlueprints.ToDictionary(r => r.blueprintId);
        _trapLookup = trapBlueprints.ToDictionary(t => t.blueprintId);
    }
    
    #region Getters

    public RoomBlueprint GetRoom(string id)
    {
        if (_roomLookup.TryGetValue(id, out var room))
            return room;
        
        Debug.LogError($"Blueprint with id {id} not found");
        return null;
    }

    public TrapBlueprint GetTrap(string id)
    {
        if (_trapLookup.TryGetValue(id, out var trap))
            return trap;
        
        Debug.LogError($"Blueprint with id {id} not found");
        return null;
    }
    
    #endregion
}