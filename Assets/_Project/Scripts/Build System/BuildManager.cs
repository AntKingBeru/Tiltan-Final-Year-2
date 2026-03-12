using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private GhostMaterialSettings ghostMaterials;
    
    [Header("Input")]
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference cancelAction;

    private BuildMode _currentMode = BuildMode.None;

    private RoomBlueprint _selectedRoom;
    private TrapBlueprint _selectedTrap;
    
    private RoomGhost _currentGhost;
    private TrapGhost _currentTrapGhost;
    
    public BuildMode CurrentMode => _currentMode;
    public RoomGhost CurrentGhost => _currentGhost;
    public TrapGhost CurrentTrapGhost => _currentTrapGhost;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        primaryAction.action.performed += OnPrimaryAction;
        cancelAction.action.performed += OnCancelBuildMode;
    }
    
    private void OnDisable()
    {
        primaryAction.action.performed -= OnPrimaryAction;
        cancelAction.action.performed -= OnCancelBuildMode;
    }

    private void OnPrimaryAction(InputAction.CallbackContext context)
    {
        switch (_currentMode)
        {
            case BuildMode.Clear:
                ClearCell();
                break;
            case BuildMode.Room:
                PlaceRoom();
                break;
            case BuildMode.Trap:
                PlaceTrap();
                break;
        }
    }

    private void OnCancelBuildMode(InputAction.CallbackContext context)
    {
        CancelBuildMode();
    }

    public void SetMode(BuildMode mode)
    {
        _currentMode = mode;

        if (mode != BuildMode.Room) DestroyGhost();
    }

    public void SelectRoom(RoomBlueprint blueprint)
    {
        _selectedRoom = blueprint;
        
        SetMode(BuildMode.Room);

        CreateRoomGhost();
    }

    public void SelectTrap(TrapBlueprint blueprint)
    {
        _selectedTrap = blueprint;
        
        SetMode(BuildMode.Trap);

        CreateTrapGhost();
    }

    private void CreateRoomGhost()
    {
        DestroyGhost();
        
        var ghostObj = Instantiate(_selectedRoom.prefab);
        
        _currentGhost = ghostObj.AddComponent<RoomGhost>();
        
        _currentGhost.Initialize(_selectedRoom, cam, ghostMaterials);
    }

    private void CreateTrapGhost()
    {
        DestroyGhost();
        
        var ghostObj = Instantiate(_selectedTrap.prefab);
        
        _currentTrapGhost = ghostObj.AddComponent<TrapGhost>();
        
        _currentTrapGhost.Initialize(_selectedTrap, cam, ghostMaterials);
    }

    private void DestroyGhost()
    {
        if (_currentGhost)
        {
            Destroy(_currentGhost.gameObject);
            _currentGhost = null;
        }
        
        if (_currentTrapGhost)
        {
            Destroy(_currentTrapGhost.gameObject);
            _currentTrapGhost = null;
        }
    }

    private void ClearCell()
    {
        var gridPos = GridUtility.GetMouseGridPosition(cam);
        
        DungeonGrid.Instance.ClearCell(gridPos);
    }

    private void PlaceRoom()
    {
        if (!_currentGhost || !_currentGhost.CanPlace()) return;
        
        if (!ResourceManager.Instance.CanAfford(
                _selectedRoom.stoneCost,
                _selectedRoom.woodCost))
            return;
        
        var pos = _currentGhost.GetGridPosition();

        var cellSize = DungeonGrid.Instance.CellSize;

        var basePos = DungeonGrid.Instance.GridToWorld(pos);
        
        var offset = new Vector3(
            (_selectedRoom.size.x - 1) * cellSize * 0.5f,
            0,
            (_selectedRoom.size.y - 1) * cellSize * 0.5f
        );

        var room = Instantiate(
            _selectedRoom.prefab,
            basePos + offset,
            Quaternion.identity
        );
        
        var roomComponent = room.GetComponent<Room>();
        
        if (roomComponent) roomComponent.Initialize(_selectedRoom, pos);

        DungeonGrid.Instance.RegisterRoom(
            pos,
            _selectedRoom.size,
            room
        );

        ResourceManager.Instance.Spend(
            _selectedRoom.stoneCost,
            _selectedRoom.woodCost
        );
    }

    public void UpgradeRoom(Room room, RoomBlueprint upgrade)
    {
        if (!ResourceManager.Instance.CanAfford(
                upgrade.stoneCost,
                upgrade.woodCost))
            return;

        var pos = room.GetGridPosition();

        ResourceManager.Instance.Spend(
            upgrade.stoneCost,
            upgrade.woodCost
        );
        
        Destroy(room.gameObject);
        
        var cellSize = DungeonGrid.Instance.CellSize;
        
        var basePos = DungeonGrid.Instance.GridToWorld(pos);
        
        var offset = new Vector3(
            (upgrade.size.x - 1) * cellSize * 0.5f,
            0,
            (upgrade.size.y - 1) * cellSize * 0.5f
        );

        var newRoom = Instantiate(
            upgrade.prefab,
            basePos + offset,
            Quaternion.identity
        );

        var roomComponent = newRoom.GetComponent<Room>();
        
        roomComponent.Initialize(upgrade, pos);
    }

    private void PlaceTrap()
    {
        if (!_currentTrapGhost || !_currentTrapGhost.CanPlace()) return;
        
        if (!ResourceManager.Instance.CanAfford(
                _selectedTrap.stoneCost,
                _selectedTrap.woodCost
                ))
            return;
        
        var anchor = _currentTrapGhost.GetAnchor();

        if (!anchor || !anchor.CanPlaceTrap()) return;
        
        var trap = Instantiate(_selectedTrap.prefab);
        
        anchor.PlaceTrap(trap);
        
        ResourceManager.Instance.Spend(
            _selectedTrap.stoneCost,
            _selectedTrap.woodCost
        );
    }

    public void CancelBuildMode()
    {
        SetMode(BuildMode.None);
        DestroyGhost();
    }
    
    public RoomBlueprint GetSelectedRoom() => _selectedRoom;
}