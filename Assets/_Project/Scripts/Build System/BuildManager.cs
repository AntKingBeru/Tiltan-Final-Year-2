using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionReference buildToggleAction;
    
    [SerializeField] private UpgradeUI upgradeUI;

    public BuildMode CurrentMode { get; private set; }
    
    public bool IsClearMode => CurrentMode == BuildMode.Clear;
    public bool IsBuildMode => CurrentMode == BuildMode.Build;
    public bool IsUpgradeMode => CurrentMode == BuildMode.Upgrade;
    
    public UpgradeUI UpgradeUI => upgradeUI;

    public RoomBlueprint SelectedRoom { get; private set; }
    public TrapBlueprint SelectedTrap { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        buildToggleAction.action.Enable();
        buildToggleAction.action.performed += OnToggleBuild;
    }
    
    private void OnDisable()
    {
        buildToggleAction.action.performed -= OnToggleBuild;
        buildToggleAction.action.Disable();
    }

    private void OnToggleBuild(InputAction.CallbackContext context)
    {
        IsBuildMode = !IsBuildMode;
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
    
    public void SetClearMode() => CurrentMode = BuildMode.Clear;
    
    public void SetBuildMode() => CurrentMode = BuildMode.Build;
    
    public void SetUpgradeMode() => CurrentMode = BuildMode.Upgrade;
    
    public void SetNone() => CurrentMode = BuildMode.None;
    
    #endregion
}