using UnityEngine;

public class InitialRoomPlacer : MonoBehaviour
{
    [Header("Room Blueprints")]
    [SerializeField] private RoomBlueprint coreRoom;
    [SerializeField] private RoomBlueprint revivalRoom;
    [SerializeField] private RoomBlueprint barracksRoom;
    [SerializeField] private RoomBlueprint storageRoom;
    
    [Header("Offsets")]
    [SerializeField] private Vector2Int corePosition = new(5, 5);
    [SerializeField] private Vector2Int revivalOffset = new(-3, 0);
    [SerializeField] private Vector2Int barracksOffset = new(3, 0);
    [SerializeField] private Vector2Int storageOffset = new(6, 0);

    public void GenerateInitialRooms()
    {
        PlaceRoom(coreRoom, corePosition);

        var revivalPos = corePosition + revivalOffset;
        PlaceRoom(revivalRoom, revivalPos);
        
        var barracksPos = corePosition + barracksOffset;
        PlaceRoom(barracksRoom, barracksPos);

        var storagePos = corePosition + storageOffset;
        PlaceRoom(storageRoom, storagePos);
    }

    private void PlaceRoom(RoomBlueprint blueprint, Vector2Int origin)
    {
        var size = blueprint.size;
        
        GridManager.Instance.ForceClearArea(origin, size);
        
        var worldPos = GridManager.Instance.GridToWorld(origin);

        var roomObj = Instantiate(
            blueprint.prefab,
            worldPos,
            Quaternion.identity
        );
        
        var room = roomObj.GetComponent<Room>();
        
        room.Initialize(origin, size, blueprint.blocksEnemies);
        
        RoomRegistry.Instance.Register(room);
        
        GridManager.Instance.OccupyArea(origin, size);
    }
}