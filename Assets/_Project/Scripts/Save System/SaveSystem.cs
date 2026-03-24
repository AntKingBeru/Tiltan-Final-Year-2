using UnityEngine;
using System.IO;
using System.Linq;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private string GetPath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{slot}.json");
    }
    
    #region Save

    public void Save(int slot, string saveName)
    {
        var data = new SaveData
        {
            saveName = saveName,

            resources = new ResourceData
            {
                stone = ResourceManager.Instance.GetStone(),
                wood = ResourceManager.Instance.GetWood()
            },

            rooms = RoomRegistry.Instance.GetAllRooms().Select(r => new RoomData
            {
                id = r.BlueprintId,
                x = r.Origin.x,
                y = r.Origin.y,
                width = r.Size.x,
                height = r.Size.y,
                rotation = r.RotationIndex,
                upgradeLevel = r.UpgradeLevel
            }).ToList(),

            traps = FindObjectsByType<Trap>(FindObjectsSortMode.None).Select(t => new TrapData
            {
                id = t.BlueprintId,
                position = t.transform.position,
                rotation = t.transform.eulerAngles,
                upgradeLevel = t.UpgradeLevel
            }).ToList(),

            minions = MinionManager.Instance.GetAll().Select(m => new MinionData
            {
                position = m.transform.position
            }).ToList()
        };
        
        File.WriteAllText(GetPath(slot), JsonUtility.ToJson(data, true));
        
        Debug.Log($"Saved to slot {slot}");
    }
    
    #endregion
    
    #region Load

    public SaveData Load(int slot)
    {
        var path = GetPath(slot);
        
        if (!File.Exists(path))
            return null;
        
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public bool HasSave(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    public void ApplySave(SaveData data)
    {
        GridManager.Instance.GenerateGrid();
        
        ResourceManager.Instance.Set(data.resources.stone, data.resources.wood);

        foreach (var r in data.rooms)
        {
            var blueprint = BlueprintDataBase.Instance.GetRoom(r.id);
            
            var pos = new Vector2Int(r.x, r.y);

            var obj = Instantiate(
                blueprint.prefab,
                GridManager.Instance.GridToWorld(pos),
                Quaternion.Euler(0f, r.rotation * 90f, 0f),
                GridManager.Instance.transform
            );

            var room = obj.GetComponent<Room>();

            room.Initialize(
                pos,
                new Vector2Int(r.width, r.height),
                blueprint.blocksEnemies,
                r.id,
                r.rotation
            );
            
            room.SetUpgradeLevel(r.upgradeLevel);
            
            RoomRegistry.Instance.Register(room);
            GridManager.Instance.OccupyArea(pos, room.Size);
        }

        foreach (var t in data.traps)
        {
            var blueprint = BlueprintDataBase.Instance.GetTrap(t.id);

            var obj = Instantiate(
                blueprint.prefab,
                t.position,
                Quaternion.Euler(t.rotation),
                GridManager.Instance.transform
            );
            
            var trap = obj.GetComponent<Trap>();
            
            trap.SetUpgradeLevel(t.upgradeLevel);
        }

        foreach (var m in data.minions)
        {
            MinionManager.Instance.SpawnMinion(m.position);
        }
        
        NavMeshManager.Instance.Rebuild();
    }
    
    #endregion
}