using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Pivots")]
    [SerializeField] private Transform[] pivots;

    [Header("Settings")]
    [SerializeField, Range(0, 3)] private int startIndex;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Input")]
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private float inputThreshold = 0.5f;
    [SerializeField] private float inputCooldown = 0.3f;

    private int _currentIndex;
    private bool _isRotating;
    
    private float _cooldownTimer;

    private void Awake()
    {
        _currentIndex = Mathf.Clamp(startIndex, 0, pivots.Length - 1);
        
        var startPoint = pivots[_currentIndex];
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

    private void OnEnable()
    {
        rotateAction.action.Enable();
    }
    
    private void OnDisable()
    {
        rotateAction.action.Disable();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void LateUpdate()
    {
        var target = pivots[_currentIndex];

        if (_isRotating)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                target.rotation,
                rotationSpeed * Time.deltaTime
            );
            
            if (Vector3.Distance(transform.position, target.position) < 0.1f &&
                Quaternion.Angle(transform.rotation, target.rotation) < 0.5f
            )
            {
                transform.position = target.position;
                transform.rotation = target.rotation;
                _isRotating = false;
            }
        }
        else
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }

    private void HandleRotation()
    {
        if (_isRotating)
            return;
        
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer > 0f)
            return;
        
        var value = rotateAction.action.ReadValue<float>();

        if (value > inputThreshold)
        {
            RotateRight();
            _cooldownTimer = inputCooldown;
        }
        else if (value < -inputThreshold)
        {
            RotateLeft();
            _cooldownTimer = inputCooldown;
        }
    }

    private void RotateRight()
    {
        _currentIndex++;
        if (_currentIndex >= pivots.Length)
            _currentIndex = 0;
        
        _isRotating = true;
    }

    private void RotateLeft()
    {
        _currentIndex--;
        if (_currentIndex < 0)
            _currentIndex = pivots.Length - 1;
        
        _isRotating = true;
    }
}