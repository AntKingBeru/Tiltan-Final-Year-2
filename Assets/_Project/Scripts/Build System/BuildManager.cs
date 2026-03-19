using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionReference buildToggleAction;

    public bool IsBuildMode { get; private set; }

    public RoomBlueprint SelectedRoom { get; private set; }
    public TrapBlueprint SelectedTrap { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        IsBuildMode = false;
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
}