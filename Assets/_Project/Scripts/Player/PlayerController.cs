using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        agent.speed = moveSpeed;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        var input = moveAction.action.ReadValue<Vector2>();

        if (input.sqrMagnitude < 0.01f)
            return;
        
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;
        
        forward.y = 0;
        right.y = 0;
        
        forward.Normalize();
        right.Normalize();
        
        var direction = forward * input.y + right * input.x;
        
        var target = transform.position + direction * moveDistance;
        
        agent.SetDestination(target);

        if (direction.sqrMagnitude > 0.01f)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}