using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public RoomBlueprint Blueprint { get; private set; }
    
    private Vector2Int _gridPosition;
    
    private TrapAnchor[] _anchors;

    public Vector2Int Size => Blueprint.size;

    public void Initialize(RoomBlueprint blueprint, Vector2Int gridPos)
    {
        Blueprint = blueprint;
        _gridPosition = gridPos;
        
        _anchors = GetComponentsInChildren<TrapAnchor>();

        foreach (var anchor in _anchors) anchor.SetRoom(this);
    }
    
    public Vector2Int GetGridPosition() => _gridPosition;
    
    public bool HasUpgrade() => Blueprint.upgrades is { Length: > 0 };

    public RoomBlueprint GetUpgrade(int index) => Blueprint.upgrades[index];

    public TrapAnchor[] GetAnchors() => _anchors;
}