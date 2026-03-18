using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform pivot;

    private int _rotationIndex;

    public void Rotate(int direction)
    {
        // +4 is so we have no negative values
        _rotationIndex = (_rotationIndex + direction + 4) % 4;
        var angle = _rotationIndex * 90f;
        
        pivot.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}