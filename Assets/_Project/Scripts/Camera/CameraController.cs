using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] pivots;
    [SerializeField] private float lerpSpeed = 5f;

    [Header("Input")]
    [SerializeField] private InputActionReference rotateAction;

    private int _index;
    private float _lastRotateTime;
    private const float Cooldown = 0.2f;

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
        if (BuildManager.Instance.IsBuildMode)
            return;

        HandleRotation();
        FollowTarget();
    }

    private void HandleRotation()
    {
        var value = rotateAction.action.ReadValue<float>();
        
        if (Mathf.Abs(value) < 0.5f)
            return;
        
        if (Time.time - _lastRotateTime < Cooldown)
            return;
        
        if (value > 0)
            _index = (_index + 1) % pivots.Length;
        else
            _index = (_index + pivots.Length - 1) % pivots.Length;
        
        _lastRotateTime = Time.time;
    }

    private void FollowTarget()
    {
        foreach (var pivot in pivots)
            pivot.position = target.position;
        
        var currentPivot = pivots[_index];

        transform.position = Vector3.Lerp(
            transform.position,
            currentPivot.position,
            lerpSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            currentPivot.rotation,
            lerpSpeed * Time.deltaTime
        );
    }
}