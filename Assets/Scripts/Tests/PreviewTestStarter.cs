using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewTestStarter : MonoBehaviour
{
    [SerializeField] private RoomBlueprint testBlueprint;
    [SerializeField] private InputActionReference startPreviewAction;

    [SerializeField] private BuildPreviewController previewController;

    private void OnEnable()
    {
        startPreviewAction.action.Enable();
        startPreviewAction.action.performed += StartPreview;
    }

    private void OnDisable()
    {
        startPreviewAction.action.performed -= StartPreview;
        startPreviewAction.action.Disable();
    }

    private void StartPreview(InputAction.CallbackContext ctx)
    {
        if (!BuildModeController.Instance.IsBuildMode)
            return;

        previewController.StartPreview(testBlueprint);
    }
}