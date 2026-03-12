using UnityEngine;
using UnityEngine.InputSystem;

public static class GridUtility
{
    public static Vector2Int GetMouseGridPosition(Camera cam)
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        var plane = new Plane(Vector3.up, Vector3.zero);
        
        if (plane.Raycast(ray, out var distance))
        {
            var worldPos = ray.GetPoint(distance);
            return DungeonGrid.Instance.WorldToGrid(worldPos);
        }
        
        return Vector2Int.zero;
    }
}