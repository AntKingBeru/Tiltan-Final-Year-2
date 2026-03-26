using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionReference buildToggleAction;
    [SerializeField] private InputActionReference cancelAction;
    
    [SerializeField] private UpgradeUI upgradeUI;

    public BuildMode CurrentMode { get; private set; }
    
    public Action<BuildMode> OnModeChanged;
    
    public bool IsClearMode => CurrentMode == BuildMode.Clear;
    public bool IsBuildMode => CurrentMode == BuildMode.Build;
    public bool IsUpgradeMode => CurrentMode == BuildMode.Upgrade;
    
    public UpgradeUI UpgradeUI => upgradeUI;

    public RoomBlueprint SelectedRoom { get; private set; }
    public TrapBlueprint SelectedTrap { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetNone();
    }

    private void OnEnable()
    {
        buildToggleAction.action.Enable();
        cancelAction.action.Enable();
        
        buildToggleAction.action.performed += OnToggleBuild;
        cancelAction.action.performed += OnCancel;
    }
    
    private void OnDisable()
    {
        buildToggleAction.action.performed -= OnToggleBuild;
        cancelAction.action.performed -= OnCancel;
        
        buildToggleAction.action.Disable();
        cancelAction.action.Disable();
    }

    private void OnToggleBuild(InputAction.CallbackContext context)
    {
        CycleMode();
    }

    private void CycleMode()
    {
        switch (CurrentMode)
        {
            case BuildMode.None:
                SetMode(BuildMode.Clear);
                break;
            case BuildMode.Clear:
                SetMode(BuildMode.Build);
                break;
            case BuildMode.Build:
                SetMode(BuildMode.Upgrade);
                break;
            case BuildMode.Upgrade:
                SetMode(BuildMode.None);
                break;
        }
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        Cancel();
    }

    public void Cancel()
    {
        SetNone();
        
        ClearSelection();
        
        if (upgradeUI)
            upgradeUI.Hide();
    }

    public void SetMode(BuildMode newMode)
    {
        if (CurrentMode == newMode)
            return;
        
        CurrentMode = newMode;

        OnModeChanged(newMode);

        OnModeChanged?.Invoke(CurrentMode);
    }

    public void SelectRoom(RoomBlueprint room)
    {
        SelectedRoom = room;
    }
    
    public void SelectTrap(TrapBlueprint trap)
    {
        SelectedTrap = trap;
        SelectedRoom = null;
    }

    public void ClearSelection()
    {
        SelectedRoom = null;
    }
    
    #region Setters
    
    public void SetClearMode() => SetMode(BuildMode.Clear);
    
    public void SetBuildMode() => SetMode(BuildMode.Build);
    
    public void SetUpgradeMode() => SetMode(BuildMode.Upgrade);
    
    public void SetNone() => SetMode(BuildMode.None);
    
    #endregion
}