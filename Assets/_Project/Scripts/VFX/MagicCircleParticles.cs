using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MagicCircleParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    
    [Header("Base Vortex")]
    [SerializeField] private float orbitalY = 3f;
    [SerializeField] private float radial = -2f;
    [SerializeField] private float upward = 1f;
    
    [Header("Pulse Burst")]
    [SerializeField] private float pulseUpward = 8f;
    [SerializeField] private float pulseDuration = 0.5f;

    private float _pulseTimer;
    private bool _isPulsing;
    private ParticleSystem.VelocityOverLifetimeModule _velocityModule;
    
    private void Awake()
    {
        _velocityModule = ps.velocityOverLifetime;
    }

    private void Start()
    {
        ApplyBaseVortex();
    }

    private void Update()
    {
        if (_isPulsing)
        {
            _pulseTimer += Time.deltaTime;
            
            if (_pulseTimer >= pulseDuration)
            {
                _isPulsing = false;
                ApplyBaseVortex();
            }
        }
    }
    
    #region Base Vortex

    private void ApplyBaseVortex()
    {
        _velocityModule.enabled = true;
        
        _velocityModule.orbitalY = orbitalY;
        _velocityModule.radial = radial;
        _velocityModule.y = upward;
    }
    
    #endregion
    
    #region Pulse

    public void TriggerPulse()
    {
        _isPulsing = true;
        _pulseTimer = 0f;

        _velocityModule.orbitalY = 0f; // Stop spinning
        _velocityModule.radial = 0f; // Stop inward pull
        _velocityModule.y = pulseUpward; // Shoot upwards

        ps.Emit(30);
    }
    
    #endregion
}