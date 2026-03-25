using UnityEngine;
using System.Collections;

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
    
    [Header("Parent")]
    [SerializeField] private Transform roomsParent;

    public void GenerateInitialRooms()
    {
        if (!roomsParent)
            return;
        
        PlaceRoom(coreRoom, corePosition);

        var revivalPos = corePosition + revivalOffset;
        PlaceRoom(revivalRoom, revivalPos);
        
        var barracksPos = corePosition + barracksOffset;
        PlaceRoom(barracksRoom, barracksPos);

        var storagePos = corePosition + storageOffset;
        var storage = PlaceRoom(storageRoom, storagePos);
        
        if (storage)
            StartCoroutine(AssignStorageNextFrame(storage.transform));
    }

    private IEnumerator AssignStorageNextFrame(Transform storage)
    {
        yield return null;
        
        if (MinionManager.Instance)
            MinionManager.Instance.SetStorage(storage);
    }

    private Room PlaceRoom(RoomBlueprint blueprint, Vector2Int origin)
    {
        if (!blueprint)
        {
            Debug.LogError("Blueprint is NULL!");
            return null;
        }

        if (!blueprint.prefab)
        {
            Debug.LogError($"Prefab missing in blueprint: {blueprint.name}");
            return null;
        }

        if (!roomsParent)
        {
            Debug.LogError("Rooms Parent is NULL!");
            return null;
        }
        
        var size = blueprint.size;
        
        GridManager.Instance.ForceClearArea(origin, size);
        
        var worldPos = GridManager.Instance.GridToWorld(origin);

        var roomObj = Instantiate(
            blueprint.prefab,
            worldPos,
            Quaternion.identity,
            roomsParent
        );
        
        var room = roomObj.GetComponent<Room>();

        if (!room)
        {
            Debug.LogError($"Room prefab missing Room component: {blueprint.prefab.name}");
            return null;
        }
        
        room.Initialize(origin, size, blueprint.blocksEnemies, blueprint.blueprintId, 0);
        
        if (RoomRegistry.Instance)
            RoomRegistry.Instance.Register(room);
        else
            Debug.LogWarning("RoomRegistry not initialized yet!");
        
        GridManager.Instance.OccupyArea(origin, size);
        
        return room;
    }
}