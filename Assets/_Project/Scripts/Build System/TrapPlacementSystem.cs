using UnityEngine;
using UnityEngine.InputSystem;

public class TrapPlacementSystem : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference placeAction;
    
    [Header("Ghost")]
    [SerializeField] private RoomGhost ghostPrefab;
    
    [Header("Settings")]
    [SerializeField] private float maxSnapDistance = 2f;
    
    [SerializeField] private Camera cam; 
    
    private RoomGhost _currentGhost;
    private TrapAnchor _currentAnchor;
    
    private void OnEnable()
    {
        placeAction.action.Enable();
        placeAction.action.performed += OnPlace;
    }

    private void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
        placeAction.action.Disable();
    }

    private void Update()
    {
        if (!BuildManager.Instance.IsBuildMode ||
            !BuildManager.Instance.SelectedTrap)
        {
            ClearGhost();
            return;
        }

        UpdateGhost();
    }
    
    #region Ghost Logic

    private void UpdateGhost()
    {
        var mouseWorld = GetMouseWorldPosition();
        
        var nearest = FindNearestAnchor(mouseWorld);

        if (!nearest)
        {
            ClearGhost();
            return;
        }
        
        _currentAnchor = nearest;

        var canPlace = !nearest.IsOccupied;
        
        if (!_currentGhost)
            _currentGhost = Instantiate(ghostPrefab);

        _currentGhost.transform.position = nearest.GetWorldPosition();
        _currentGhost.transform.rotation = nearest.transform.rotation;
        _currentGhost.transform.localScale = Vector3.one;
        
        _currentGhost.SetValidity(canPlace);
    }

    private void ClearGhost()
    {
        if (_currentGhost)
        {
            Destroy(_currentGhost.gameObject);
            _currentGhost = null;
        }
        
        _currentAnchor = null;
    }
    
    #endregion
    
    #region Placement

    private void OnPlace(InputAction.CallbackContext context)
    {
        if (!_currentAnchor || _currentAnchor.IsOccupied)
            return;

        var trap = BuildManager.Instance.SelectedTrap;
        
        Instantiate(
            trap.prefab,
            _currentAnchor.GetWorldPosition(),
            _currentAnchor.transform.rotation
        );
        
        _currentAnchor.SetOccupied(true);
    }
    
    #endregion
    
    #region Helpers

    private TrapAnchor FindNearestAnchor(Vector3 position)
    {
        var anchors = TrapAnchorRegistry.Instance.GetAllAnchors();

        TrapAnchor best = null;
        var bestDist = float.MaxValue;

        foreach (var anchor in anchors)
        {
            if (!anchor)
                continue;

            var dist = Vector3.Distance(position, anchor.GetWorldPosition());

            if (dist < bestDist && dist <= maxSnapDistance)
            {
                best = anchor;
                bestDist = dist;
            }
        }
        
        return best;
    }

    private Vector3 GetMouseWorldPosition()
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out var hit, 100f))
            return hit.point;
        
        return Vector3.zero;
    }
    
    #endregion
}