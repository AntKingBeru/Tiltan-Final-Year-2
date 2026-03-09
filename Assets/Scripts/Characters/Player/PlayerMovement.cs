using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash(SpeedParam);
    private const string SpeedParam = "Speed";
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private InputActionReference targetInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera mainCamera;

    private bool _moveRequested;
    private float _speed;

    private void Start()
    {
        targetInput.action.performed += OnTarget;
    }

    private void OnDestroy()
    {
        targetInput.action.performed -= OnTarget;
    }

    private void Update()
    {
        _speed = agent.velocity.magnitude;
        _speed = Mathf.InverseLerp(_speed, 0f, 1f);
        
        animator.SetFloat(Speed, _speed);
        
        if (!_moveRequested) return;
        _moveRequested = false;

        if (IsPointerOverUI()) return;
        
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return;
        
        agent.SetDestination(hit.point);
    }

    private void OnTarget(InputAction.CallbackContext context)
    {
        _moveRequested = true;
    }

    private static bool IsPointerOverUI()
    {
        return EventSystem.current && EventSystem.current.IsPointerOverGameObject();
    }
}