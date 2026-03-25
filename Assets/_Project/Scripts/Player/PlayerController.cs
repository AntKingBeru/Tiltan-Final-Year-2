using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform carryAnchor;

    private float _speed;
    private bool _moveRequested;
    
    private EnemyCorpse _corpse;
    
    public bool HasCorpse => _corpse;

    private void OnEnable()
    {
        moveAction.action.Enable();

        moveAction.action.performed += OnTarget;
    }
    
    private void OnDisable()
    {
        moveAction.action.performed -= OnTarget;
        
        moveAction.action.Disable();
    }

    private void Update()
    {
        _speed = agent.velocity.magnitude;
        _speed = Mathf.Clamp(_speed, 0f, 1f);

        if (!_moveRequested)
            return;
        
        _moveRequested = false;

        if (BuildManager.Instance.IsBuildMode)
            return;

        if (BuildManager.Instance.IsClearMode)
        {
            TryClear();
            return;
        }

        if (IsPointerOverUI())
            return;
        
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (!Physics.Raycast(ray, out var hit, 100f))
            return;
        
        agent.SetDestination(hit.point);
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current && EventSystem.current.IsPointerOverGameObject();
    }

    private void TryClear()
    {
        if (IsPointerOverUI())
            return;
        
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit, 100f))
            return;

        var gridPos = GridManager.Instance.WorldToGrid(hit.point);
        GridManager.Instance.ClearCell(gridPos);
    }
    
    public bool TryPickUpCorpse(EnemyCorpse corpse)
    {
        if (_corpse)
            return false;
        
        corpse.SetPlayer(this);

        _corpse = corpse;

        corpse.AttachTo(carryAnchor);

        return true;
    }

    public EnemyCorpse DropCorpse()
    {
        var corpse = _corpse;
        _corpse = null;
        return corpse;
    }

    private void OnTarget(InputAction.CallbackContext obj)
    {
        _moveRequested = true;
    }
}