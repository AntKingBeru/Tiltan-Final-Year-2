using UnityEngine;

public class MinimapIconScale : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private float baseScale = 2f;

    private void LateUpdate()
    {
        var zoom = minimapCamera.orthographicSize;
        transform.localScale = Vector3.one * (baseScale * zoom * 0.075f);
    }
}