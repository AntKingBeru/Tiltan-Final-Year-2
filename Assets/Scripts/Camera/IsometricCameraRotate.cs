using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricCameraRotate : MonoBehaviour
{
    [Header("Camera Points")]
    [SerializeField] private Transform[] cameraPoints;
    
    [Header("Start Settings")]
    [SerializeField, Range(0, 3)] private int startIndex;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference rotateLeftInput;
    [SerializeField] private InputActionReference rotateRightInput;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    private int _currentIndex;
    private bool _isTransitioning;

    private void Awake()
    {
        _currentIndex = Mathf.Clamp(startIndex, 0, cameraPoints.Length - 1);
        
        var startPoint = cameraPoints[_currentIndex];
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

    private void OnEnable()
    {
        rotateLeftInput.action.performed += RotateLeft;
        rotateRightInput.action.performed += RotateRight;
        
        rotateLeftInput.action.actionMap.Enable();
    }

    private void OnDisable()
    {
        rotateLeftInput.action.performed -= RotateLeft;
        rotateRightInput.action.performed -= RotateRight;
    }

    private void LateUpdate()
    {
        var target = cameraPoints[_currentIndex];

        if (_isTransitioning)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime);
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                target.rotation,
                rotationSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, target.position) < 0.1f &&
                Quaternion.Angle(transform.rotation, target.rotation) < 0.5f)
            {
                transform.position = target.position;
                transform.rotation = target.rotation;
                _isTransitioning = false;
            }
        }
        else
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
    
    private void RotateLeft(InputAction.CallbackContext context)
    {
        _currentIndex--;
        if (_currentIndex < 0) _currentIndex = cameraPoints.Length - 1;
        
        _isTransitioning = true;
    }
    
    private void RotateRight(InputAction.CallbackContext context)
    {
        _currentIndex++;
        if (_currentIndex >= cameraPoints.Length) _currentIndex = 0;
        
        _isTransitioning = true;
    }
}