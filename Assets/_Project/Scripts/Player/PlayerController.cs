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
    
    [SerializeField] private NavMeshAgent agent;

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
        
        var direction = new Vector3(input.x, 0, input.y);

        if (direction.sqrMagnitude > 0.01f)
        {
            var target = transform.position + direction;
            agent.SetDestination(target);
        }
    }
}