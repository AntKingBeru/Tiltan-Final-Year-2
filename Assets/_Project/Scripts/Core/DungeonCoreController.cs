using UnityEngine;

public class DungeonCoreController : MonoBehaviour
{
    private const string EmissionColorName = "_EmissionColor";
    private static readonly int EmissionColorID = Shader.PropertyToID(EmissionColorName);
    
    [Header("Height Wobble")]
    [SerializeField] private float minY = 2f;
    [SerializeField] private float maxY = 2.5f;
    
    [Header("Health")]
    [Range(0, 1)]
    [SerializeField] private float healthPercent = 1f;
    
    [Header("Wobble Speed")]
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 3f;
    
    [Header("Pulse")]
    [SerializeField] private float pulseAmount = 0.1f;
    [SerializeField] private float pulseSpeed = 2f;
    
    [Header("Color")]
    [SerializeField] private Color colorA = Color.cyan;
    [SerializeField] private Color colorB = new Color(0.3f, 0.8f, 1f);
    [SerializeField] private float emissionIntensity = 2f;
    
    [SerializeField] private Renderer rend;

    private float _time;
    private Vector3 _startPos;
    private Vector3 _baseScale;
    private MaterialPropertyBlock _block;

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
    }
    
    private void Start()
    {
        _startPos = transform.position;
        _baseScale = transform.localScale;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        HandleWobble();
        HandlePulse();
        HandleColor();
    }

    private void HandleWobble()
    {
        var speed = Mathf.Lerp(
            minSpeed,
            maxSpeed,
            1f - healthPercent
        );

        var y = Mathf.Lerp(
            minY,
            maxY,
            (Mathf.Sin(_time * speed) + 1f) * 0.5f
        );

        transform.position = new Vector3(
            _startPos.x,
            y,
            _startPos.z
        );
    }

    private void HandlePulse()
    {
        var scale = 1 + Mathf.Sin(_time * pulseSpeed) * pulseAmount;
        transform.localScale = _baseScale * scale;
    }

    private void HandleColor()
    {
        var t = (Mathf.Sin(_time) + 1f) * 0.5f;

        var c = Color.Lerp(
            colorA,
            colorB,
            t
        ) * emissionIntensity;
        
        rend.GetPropertyBlock(_block);
        _block.SetColor(EmissionColorID, c);
        rend.SetPropertyBlock(_block);
    }

    public void SetHealthPercent(float value) => healthPercent = Mathf.Clamp01(value);
}