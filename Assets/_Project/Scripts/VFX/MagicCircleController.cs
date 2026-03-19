using UnityEngine;

public class MagicCircleController : MonoBehaviour
{
    private const string EmissionColorName = "_EmissionColor";
    private static readonly int EmissionColorID = Shader.PropertyToID(EmissionColorName);

    [Header("References")]
    [SerializeField] private Transform outerRing;
    [SerializeField] private Transform middleRing;
    [SerializeField] private Transform innerRing;
    [SerializeField] private Renderer[] outerRenderers;
    [SerializeField] private Renderer[] middleRenderers;
    [SerializeField] private Renderer[] innerRenderers;
    [SerializeField] private Renderer[] glyphRenderers;
    
    [Header("Rotation")]
    [SerializeField] private float outerSpeed = 20f;
    [SerializeField] private float middleSpeed = -30f;
    [SerializeField] private float innerSpeed = 40f;

    [Header("Color Shift")]
    [SerializeField] private Color colorA = Color.cyan;
    [SerializeField] private Color colorB = Color.magenta;
    [SerializeField] private float colorSpeed = 2f;
    [SerializeField] private float emissionIntensity = 1.5f;

    [Header("Pulse")]
    [SerializeField] private float pulseScaleAmount = 0.2f;
    [SerializeField] private float pulseSpeed = 6f;
    
    [Header("Flicker")]
    [SerializeField] private float flickerIntensity = 0.1f;
    
    [SerializeField] private MagicCircleParticles particlesController;

    private MaterialPropertyBlock _block;
    private float _colorTime;
    private float _pulseTime;
    private bool _isPulsing;
    private Vector3 _baseScale;

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
    }
    
    private void Start()
    {
        _baseScale = transform.localScale;
    }

    private void Update()
    {
        HandleRotation();
        HandleColorShiftAndFlicker();
        HandlePulse();
    }
    
    #region Rotation

    private void HandleRotation()
    {
        outerRing.Rotate(Vector3.up * (outerSpeed * Time.deltaTime));
        middleRing.Rotate(Vector3.up * (middleSpeed * Time.deltaTime));
        innerRing.Rotate(Vector3.up * (innerSpeed * Time.deltaTime));
    }
    
    #endregion
    
    #region Color Shift

    private void HandleColorShiftAndFlicker()
    {
        _colorTime += Time.deltaTime * colorSpeed;
        
        var t = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(_colorTime, 1f));

        var baseColor = Color.Lerp(
            colorA,
            colorB,
            t
        );

        ApplyEmission(outerRenderers, baseColor * (emissionIntensity * 0.6f));
        
        ApplyEmission(middleRenderers, baseColor * emissionIntensity);
        
        ApplyEmission(innerRenderers, baseColor * (emissionIntensity * 1.5f));

        var flicker = 1 + Random.Range(-flickerIntensity, flickerIntensity);
        ApplyEmission(glyphRenderers, baseColor * (emissionIntensity * flicker));
    }

    private void ApplyEmission(Renderer[] rends, Color color)
    {
        foreach (var r in rends)
        {
            r.GetPropertyBlock(_block);
            _block.SetColor(EmissionColorID, color);
            r.SetPropertyBlock(_block);
        }
    }
    
    #endregion
    
    #region Pulse

    public void TriggerPulse()
    {
        _isPulsing = true;
        _pulseTime = 0f;

        particlesController.TriggerPulse();
    }

    private void HandlePulse()
    {
        if (!_isPulsing)
            return;
        
        _pulseTime += Time.deltaTime * pulseSpeed;
        
        var scale = 1 + Mathf.Sin(_pulseTime) * pulseScaleAmount;
        transform.localScale = _baseScale * scale;

        if (pulseSpeed >= Mathf.PI * 2)
        {
            _isPulsing = false;
            transform.localScale = _baseScale;
        }
    }
    
    #endregion
}