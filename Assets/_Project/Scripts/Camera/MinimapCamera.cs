using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Camera cam;
    
    [Header("Settings")]
    [SerializeField] private float height = 75f;
    [SerializeField] private float followSmooth = 15f;

    private void LateUpdate()
    {
        if (!target)
            return;

        var desiredPos = new Vector3(
            target.position.x,
            height,
            target.position.z
        );
        
        var time = Mathf.Clamp01(followSmooth * Time.deltaTime);
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            time
        );
    }
}