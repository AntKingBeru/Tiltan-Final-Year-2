using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private InputActionReference harvestAction;
    
    private IHarvestable _currentHarvestPoint;
    
    private void Start()
    {
        harvestAction.action.performed += OnHarvest;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHarvestable>(out var harvestPoint))
        {
            _currentHarvestPoint = harvestPoint;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IHarvestable>(out var harvestPoint) && _currentHarvestPoint == harvestPoint)
        {
            _currentHarvestPoint = null;
        }
    }

    private void OnHarvest(InputAction.CallbackContext context)
    {
        if (_currentHarvestPoint == null) return;
        if (!_currentHarvestPoint.CanHarvest()) return;
        
        _currentHarvestPoint.Harvest();
        _currentHarvestPoint = null;
    } 
}