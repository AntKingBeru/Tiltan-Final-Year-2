using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorSelector : MonoBehaviour
{
    [SerializeField] private InputActionReference selectAction;
    [SerializeField] private BuildPreviewController previewController;
    [SerializeField] private Camera mainCamera;

    private void OnEnable()
    {
        selectAction.action.Enable();
        selectAction.action.performed += SelectAnchor;
    }

    private void OnDisable()
    {
        selectAction.action.performed -= SelectAnchor;
        selectAction.action.Disable();
    }

    private void SelectAnchor(InputAction.CallbackContext context)
    {
        if (!BuildModeController.Instance.IsBuildMode) return;
        
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit)) return;

        if (!hit.collider.TryGetComponent(out RoomAnchor anchor)) return;

        if (anchor.IsOccupied) return;

        var gridPos = anchor.GetGridPosition(anchor.GetRoom().GridPosition);
        
        previewController.StartPreviewAt(gridPos);
    }
}