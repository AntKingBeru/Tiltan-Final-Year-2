using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] pivots;
    [SerializeField] private float lerpSpeed = 5f;

    [Header("Input")]
    [SerializeField] private InputActionReference rotateAction;
    
    [SerializeField] private Vector3 offset = new Vector3(0f, 25f, -15f);

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
        var pivot = pivots[_index];

        var desiredPosition = target.position + pivot.rotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            lerpSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            pivot.rotation,
            lerpSpeed * Time.deltaTime
        );
    }
}