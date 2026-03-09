using UnityEngine;
using UnityEngine.InputSystem;

public class BuildPreviewController : MonoBehaviour
{
    [Header("Preview Materials")]
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    
    [Header("Controls")]
    [SerializeField] private InputActionReference confirmAction;
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference cancelAction;
    
    [SerializeField] private Camera mainCamera;

    private Renderer[] _previewRenderers;
    private GameObject _previewObject;
    private RoomBlueprint _currentBlueprint;
    private Vector2Int _currentGridPosition;
    private int _currentRotation;

    private bool _isPlacing;

    private void OnEnable()
    {
        confirmAction.action.Enable();
        rotateAction.action.Enable();
        cancelAction.action.Enable();
        confirmAction.action.performed += ConfirmPlacement;
        rotateAction.action.performed += RotatePreview;
        cancelAction.action.performed += CancelPlacement;
        BuildModeController.OnBuildModeChanged += OnBuildModeChanged;
    }

    private void OnDisable()
    {
        BuildModeController.OnBuildModeChanged -= OnBuildModeChanged;
        
        confirmAction.action.performed -= ConfirmPlacement;
        rotateAction.action.performed -= RotatePreview;
        cancelAction.action.performed -= CancelPlacement;
        confirmAction.action.Disable();
        rotateAction.action.Disable();
        cancelAction.action.Disable();
    }

    private void Update()
    {
        if (!_isPlacing) return;

        UpdatePreviewPosition();
    }

    private void OnBuildModeChanged(bool active)
    {
        if (!active) CancelPreview();
    }

    public void StartPreview(RoomBlueprint blueprint)
    {
        CancelPreview();

        _currentBlueprint = blueprint;

        _previewObject = Instantiate(blueprint.prefab);
        
        SetPreviewMaterial(validMaterial);
        
        _isPlacing = true;
    }

    public void StartPreviewAt(Vector2Int gridPosition)
    {
        _currentGridPosition = gridPosition;
        
        var worldPos = DungeonGrid.Instance.GridToWorld(_currentGridPosition);
        
        _previewObject.transform.position = worldPos;
    }

    private void UpdatePreviewPosition()
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (!Physics.Raycast(ray, out var hit)) return;
        
        _currentGridPosition = DungeonGrid.Instance.WorldToGrid(hit.point);
        
        var worldPos = DungeonGrid.Instance.GridToWorld(_currentGridPosition);
        
        _previewObject.transform.position = worldPos;

        var valid = DungeonGrid.Instance.IsAreaFree(
            _currentGridPosition,
            _currentBlueprint.size
        );
        
        SetPreviewMaterial(valid ? validMaterial : invalidMaterial);
    }

    private void RotatePreview(InputAction.CallbackContext context)
    {
        if (!_isPlacing) return;

        _currentRotation += 90;
        
        _previewObject.transform.rotation = Quaternion.Euler(0f, _currentRotation, 0f);
    }

    private Vector2Int GetRotatedSize()
    {
        if (_currentRotation % 180 == 0) return _currentBlueprint.size;

        return new Vector2Int(
            _currentBlueprint.size.y,
            _currentBlueprint.size.x
        );
    }

    private void ConfirmPlacement(InputAction.CallbackContext context)
    {
        if (!_isPlacing) return;
        
        var size = GetRotatedSize();
        
        if (!DungeonGrid.Instance.IsAreaFree(_currentGridPosition, size)) return;

        PlaceRoom(size);
    }

    private void PlaceRoom(Vector2Int size)
    {
        var worldPos = DungeonGrid.Instance.GridToWorld(_currentGridPosition);

        var roomObj = Instantiate(
            _currentBlueprint.prefab,
            worldPos,
            Quaternion.Euler(0f, _currentRotation, 0f)
        );

        var room = roomObj.GetComponent<RoomInstance>();
        room.Initialize(_currentBlueprint, _currentGridPosition);

        DungeonGrid.Instance.SetAreaOccupied(
            _currentGridPosition,
            size,
            true
        );
    }

    private void CancelPlacement(InputAction.CallbackContext context)
    {
        if (!_isPlacing) return;
        
        CancelPreview();
    }

    private void SetPreviewMaterial(Material mat)
    {
        _previewRenderers = _previewObject.GetComponentsInChildren<Renderer>(); 
        
        foreach (var r in  _previewRenderers) r.material = mat;
    }

    public void CancelPreview()
    {
        if (_previewObject) Destroy(_previewObject);
        
        _previewObject = null;
        _currentBlueprint = null;
        _isPlacing = false;
    }
}