using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        var pos = target.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}