using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offset = 75f;
    [SerializeField] private float followSmooth = 15f;
    
    [SerializeField] private InputActionReference zoomAction;
    [SerializeField] private float zoomSpeed = 10f, minZoom = 10f, maxZoom = 30f, zoomSmoothTime = 0.15f;
    [SerializeField] private float startZoom = 15f;
    
    [SerializeField] private Camera zoomCamera;
    
    private float _targetZoom, _zoomVelocity;

    private void Start()
    {
        zoomCamera.orthographicSize = startZoom;
        _targetZoom = zoomCamera.orthographicSize;
    }

    private void OnEnable()
    {
        if (!zoomAction.action.enabled) zoomAction.action.Enable();
    }
    
    private void OnDisable()
    {
        zoomAction.action.Disable();
    }

    private void LateUpdate()
    {
        FollowTarget();
        HandleZoom();
    }

    private void FollowTarget()
    {
        if (!target) return;
        
        var desiredPos = new Vector3(target.position.x, offset, target.position.z);
        
        var time = Mathf.Clamp01(followSmooth * Time.deltaTime);
        
        transform.position = Vector3.Lerp(transform.position, desiredPos, time);
    }

    private void HandleZoom()
    {
        var zoomInput = zoomAction.action.ReadValue<float>();

        if (Mathf.Approximately(zoomInput, 0f)) return;
        
        _targetZoom = Mathf.Clamp(_targetZoom - zoomInput * zoomSpeed, minZoom, maxZoom);
            
        zoomCamera.orthographicSize = Mathf.SmoothDamp(zoomCamera.orthographicSize, _targetZoom, ref _zoomVelocity, zoomSmoothTime);
    }
}