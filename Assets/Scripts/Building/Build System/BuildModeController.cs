using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class BuildModeController : MonoBehaviour
{
    public static BuildModeController Instance {get; private set;}
    
    public static event Action<bool> OnBuildModeChanged;
    
    [SerializeField] private InputActionReference toggleBuildModeAction;
    // [SerializeField] private Texture2D normalCursor;
    // [SerializeField] private Texture2D buildCursor;

	public bool IsBuildMode {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        toggleBuildModeAction.action.Enable();
        toggleBuildModeAction.action.performed += ToggleBuildMode;
    }

    private void OnDisable()
    {
        toggleBuildModeAction.action.performed -= ToggleBuildMode;
        toggleBuildModeAction.action.Disable();
    }

    private void ToggleBuildMode(InputAction.CallbackContext context)
	{
		SetBuildMode(!IsBuildMode);
	}

    public void SetBuildMode(bool value)
    {
        if (IsBuildMode == value) return;
        
        IsBuildMode = value;
        Debug.Log($"Build Mode: {IsBuildMode}");

        // Cursor.SetCursor(
        //     IsBuildMode ? buildCursor : normalCursor,
        //     Vector2.zero,
        //     CursorMode.Auto
        // );
        
        OnBuildModeChanged?.Invoke(value);
    }
}