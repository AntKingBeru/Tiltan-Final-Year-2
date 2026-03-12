using UnityEngine;

public class TrapAnchor : MonoBehaviour
{
    private Room _room;

    private GameObject _currentTrap;

    public void SetRoom(Room room)
    {
        _room = room;
    }

    public bool CanPlaceTrap()
    {
        return !_currentTrap;
    }

    public void PlaceTrap(GameObject trap)
    {
        if (!CanPlaceTrap()) return;
        
        trap.transform.SetParent(transform);
        
        trap.transform.localPosition = Vector3.zero;
        trap.transform.localRotation = Quaternion.identity;

        _currentTrap = trap;
    }
    
    public bool HasTrap() => _currentTrap;
}