using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapPlacement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionReference placeAction;
    [SerializeField] private TrapBlueprint selectedTrap;

    private void OnEnable()
    {
        placeAction.action.performed += OnPlace;
    }

    private void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
    }

    private void OnPlace(InputAction.CallbackContext context)
    {
        var anchor = FindClosestAnchor();

        if (anchor == null) return;

        if (!anchor.CanPlaceTrap()) return;
        
        var trap = Instantiate(selectedTrap.prefab);
        
        anchor.PlaceTrap(trap);
    }

    private TrapAnchor FindClosestAnchor()
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out var hit, 100f))
        {
            var anchor = hit.collider.GetComponent<TrapAnchor>();

            return anchor;
        }

        return null;
    }
}