using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

        if (IsPointerOverUI())
            return;

        HandleClick();
    }

    private bool IsPointerOverUI()
    {
        if (!EventSystem.current)
            return false;

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        
        return results.Count > 0;
    }

    private void HandleClick()
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit, 100f))
            return;

        var gridPos = GridManager.Instance.WorldToGrid(hit.point);

        switch (BuildManager.Instance.CurrentMode)
        {
            case BuildMode.Clear:
                GridManager.Instance.ClearCell(gridPos);
                break;
            case BuildMode.Upgrade:
                HandleUpgrade(hit);
                break;
            case BuildMode.Build:
                break;
            case BuildMode.None:
            default:
                agent.SetDestination(hit.point);
                break;
        }
    }

    private void HandleUpgrade(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<Room>(out var room))
            room.Upgrade();
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