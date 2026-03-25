using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class PlayerControls : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;
    
    [SerializeField] private PlayerController player;
    
    private readonly List<IInteractable> _interactables = new();
    
    private void OnEnable()
    {
        interactAction.action.Enable();

        interactAction.action.performed += OnInteract;
    }
    
    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        
        interactAction.action.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
            _interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
            _interactables.Remove(interactable);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        foreach (var interactable in _interactables.Where(interactable => interactable != null && interactable.CanInteract(player)))
        {
            interactable.Interact(player);
            break;
        }
    }
}