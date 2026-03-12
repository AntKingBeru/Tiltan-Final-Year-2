using UnityEngine;
using UnityEngine.InputSystem;

public class CellClearTool : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionReference clearAction;

    private void OnEnable()
    {
        clearAction.action.performed += OnClearCell;
    }

    private void OnDisable()
    {
        clearAction.action.performed -= OnClearCell;
    }

    private void OnClearCell(InputAction.CallbackContext context)
    {
        var gridPos = GridUtility.GetMouseGridPosition(cam);
        
        var success = DungeonGrid.Instance.ClearCell(gridPos);

        if (success)
        {
            Debug.Log($"Cleared cell {gridPos}");
        }
    }
}