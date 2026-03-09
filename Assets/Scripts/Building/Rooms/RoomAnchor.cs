using UnityEngine;

public class RoomAnchor : MonoBehaviour
{
    public Vector2Int gridOffset;

    public bool IsOccupied { get; private set; }

    [SerializeField] private RoomInstance room;

    public Vector2Int GetGridPosition(Vector2Int roomOrigin)
    {
        return roomOrigin + gridOffset;
    }
    
    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }

    public RoomInstance GetRoom()
    {
        return room;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
#endif
}