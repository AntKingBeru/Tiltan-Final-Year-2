using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Camera minimapCam;
    
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float zoomSpeed = 40f;
    
    private Vector2 _moveInput;
    private float _zoomInput;
    
    private void Start()
    {
        var grid = DungeonGrid.Instance;

        var center = new Vector3(
            grid.Width * grid.CellSize * 0.5f,
            minimapCam.transform.position.y,
            grid.Height * grid.CellSize * 0.5f
        );
        
        minimapCam.transform.position = center;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        _zoomInput = context.ReadValue<float>();
    }

    private void Update()
    {
        if (!minimapCam) return;
        
        HandleMovement();
        HandleZoom();

        _zoomInput = 0;
    }

    private void HandleMovement()
    {
        var move = new Vector3(
            _moveInput.x,
            0,
            _moveInput.y
        );
        
        minimapCam.transform.position += move * moveSpeed * Time.deltaTime;
        
        var grid = DungeonGrid.Instance;
        
        var pos = minimapCam.transform.position;
        
        pos.x = Mathf.Clamp(pos.x, 0, grid.Width * grid.CellSize);
        pos.y = Mathf.Clamp(pos.y, 0, grid.Height * grid.CellSize);
        
        minimapCam.transform.position = pos;
    }

    private void HandleZoom()
    {
        if (Mathf.Abs(_zoomInput) < 0.01f) return;
        
        minimapCam.orthographicSize += _zoomInput * zoomSpeed * Time.deltaTime;
        
        minimapCam.orthographicSize = Mathf.Clamp(minimapCam.orthographicSize, 10f, 150f);
    }
}