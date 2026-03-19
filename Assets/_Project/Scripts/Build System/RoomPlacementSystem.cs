using UnityEngine;
using UnityEngine.InputSystem;

public class RoomPlacementSystem : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference placeAction;
    
    [Header("References")]
    [SerializeField] private Camera dungeonCamera;
    [SerializeField] private LayerMask gridLayer;
    
    [Header("Ghost")]
    [SerializeField] private RoomGhost ghostPrefab;

    [Header("Settings")]
    [SerializeField] private float rotationCooldown = 0.2f;

    private RoomGhost _currentGhost;
    private Vector2Int _currentOrigin;
    private int _rotationIndex;
    private float _lastRotationTime;

    private void OnEnable()
    {
        rotateAction.action.Enable();
        placeAction.action.Enable();
        
        placeAction.action.performed += OnPlace;
    }

    private void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
        
        rotateAction.action.Disable();
        placeAction.action.Disable();
    }

    private void Update()
    {
        if (!BuildManager.Instance.IsBuildMode ||
            !BuildManager.Instance.SelectedRoom)
        {
            ClearGhost();
            return;
        }

        HandleRotation();
        UpdateGhost();
    }
    
    #region Rotation

    private void HandleRotation()
    {
        var value = rotateAction.action.ReadValue<float>();

        if (Mathf.Abs(value) < 0.5f)
            return;

        if (Time.time - _lastRotationTime < rotationCooldown)
            return;
        
        if (value > 0f)
            _rotationIndex = (_rotationIndex + 1) % 4;
        else
            _rotationIndex = (_rotationIndex + 3) % 4;
        
        _lastRotationTime = Time.time;
    }
    
    #endregion
    
    #region Ghost Logic

    private void UpdateGhost()
    {
        var ray = dungeonCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit, 100f, gridLayer))
        {
            ClearGhost();
            return;
        }
        
        var gridPos = GridManager.Instance.WorldToGrid(hit.point);
        
        var room = BuildManager.Instance.SelectedRoom;
        var size = GetRotatedSize(room.size);

        _currentOrigin = gridPos;
        
        var gridValid = GridManager.Instance.CanPlaceRoom(_currentOrigin, size);
        var costValid = ResourceManager.Instance.CanAfford(room.stoneCost, room.woodCost);
        
        var canPlace = gridValid && costValid;
        
        // Spawn ghost if needed
        if (!_currentGhost)
            _currentGhost = Instantiate(ghostPrefab);
        
        // Position
        var worldPos = GridManager.Instance.GridToWorld(_currentOrigin);
        _currentGhost.transform.position = worldPos;
        
        // Rotation
        _currentGhost.transform.rotation = Quaternion.Euler(0f, _rotationIndex * 90f, 0f);
        
        // Scale to match room size
        _currentGhost.transform.localScale = new Vector3(
            size.x * GridManager.Instance.CellSize,
            1f,
            size.y * GridManager.Instance.CellSize
        );
        
        // Color feedback
        _currentGhost.SetValidity(canPlace);
    }

    private void ClearGhost()
    {
        if (_currentGhost)
        {
            Destroy(_currentGhost.gameObject);
            _currentGhost = null;
        }
    }
    
    #endregion
    
    #region Placement Logic
    
    private void OnPlace(InputAction.CallbackContext context)
    {
        TryPlace();
    }

    private void TryPlace()
    {
        var room = BuildManager.Instance.SelectedRoom;
        var size = GetRotatedSize(room.size);
        
        if (!GridManager.Instance.CanPlaceRoom(_currentOrigin, size))
            return;
        
        PlaceRoom(room, _currentOrigin, size);
    }

    private void PlaceRoom(RoomBlueprint room, Vector2Int origin, Vector2Int size)
    {
        if (!ResourceManager.Instance.TrySpend(room.stoneCost, room.woodCost))
            return;
        
        var worldPos = GridManager.Instance.GridToWorld(origin);

        var roomObj = Instantiate(
            room.prefab,
            worldPos,
            Quaternion.Euler(0f, _rotationIndex * 90f, 0f)
        );
        
        var roomComponent = roomObj.GetComponent<Room>();

        if (roomComponent)
        {
            roomComponent.Initialize(origin, size, room.blocksEnemies);
            RoomRegistry.Instance.Register(roomComponent);
        }
        
        GridManager.Instance.OccupyArea(origin, size);
        
        // NavMeshManager.Instance.Rebuild();
    }
    
    #endregion
    
    #region Helpers

    private Vector2Int GetRotatedSize(Vector2Int size)
    {
        if (_rotationIndex % 2 == 0)
            return size;
        
        return new Vector2Int(size.y, size.x);
    }
    
    #endregion
}