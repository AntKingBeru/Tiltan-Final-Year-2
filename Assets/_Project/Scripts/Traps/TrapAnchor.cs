using UnityEngine;

public class TrapAnchor : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    private void OnEnable()
    {
        TrapAnchorRegistry.Instance?.Register(this);
    }

    private void OnDisable()
    {
        TrapAnchorRegistry.Instance?.Unregister(this);
    }
    
    public void SetOccupied(bool value) => IsOccupied = value;
    
    public Vector3 GetWorldPosition() => transform.position;
}