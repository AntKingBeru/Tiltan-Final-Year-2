using UnityEngine;
using UnityEngine.InputSystem;

public class RoomPlacement : MonoBehaviour
{
    [SerializeField] private InputActionReference placeAction;

    private void OnEnable()
    {
        placeAction.action.performed += OnPlace;
    }

    private void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
    }

    private void OnPlace(InputAction.CallbackContext context)
    {
        var ghost = BuildManager.Instance.CurrentGhost;

        if (!ghost || !ghost.CanPlace()) return;
        
        var blueprint = BuildManager.Instance.GetSelectedRoom();

        var pos = ghost.GetGridPosition();
        
        var cellSize = DungeonGrid.Instance.CellSize;
        
        var basePos = DungeonGrid.Instance.GridToWorld(pos);

        var offset = new Vector3(
            (blueprint.size.x - 1) * cellSize * 0.5f,
            0,
            (blueprint.size.y - 1) * cellSize * 0.5f
        );

        var room = Instantiate(
            blueprint.prefab,
            basePos + offset,
            Quaternion.identity
        );
        
        var roomComponent = room.GetComponent<Room>();

        if (roomComponent) roomComponent.Initialize(blueprint, pos);
        
        MarkCellsOccupied(pos, blueprint.size, room);
    }

    private void MarkCellsOccupied(Vector2Int start, Vector2Int size, GameObject room)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var pos = start + new Vector2Int(x, y);
                
                var cell = DungeonGrid.Instance.GetCell(pos);
                
                cell.IsOccupied = true;
                cell.Room = room;
            }
        }
    }
}