using UnityEngine;

public class RoomGhost : MonoBehaviour
{
    private const string BaseColorName = "_BaseColor";
    private const string EmissionColorName = "_EmissionColor";
    
    [Header("Settings")]
    [SerializeField] private bool useEmission = true;
    [SerializeField] private float emissionMultiplier = 2f;
    
    [Header("Renderers")]
    [SerializeField] private Renderer[] renderers;
    
    private MaterialPropertyBlock _block;

    // Shader property IDs (URP)
    private static readonly int BaseColorID = Shader.PropertyToID(BaseColorName);
    private static readonly int EmissionColorID = Shader.PropertyToID(EmissionColorName);

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
    }

    // <summary>
    // Sets the color of the ghost (supports transparency + emission)
    // </summary>
    public void SetColor(Color color)
    {
        if (renderers == null || renderers.Length == 0)
            return;

        foreach (var r in renderers)
        {
            if (!r)
                continue;
            
            r.GetPropertyBlock(_block);
            
            // Base color (handles transparency using alpha)
            _block.SetColor(BaseColorID, color);
            
            // Optional emission for glow effect
            if (useEmission)
                _block.SetColor(EmissionColorID, color * emissionMultiplier);
            
            r.SetPropertyBlock(_block);
        }
    }
    
    // <summary>
    // Convenience method to set the color by validity
    // </summary>
    public void SetValidity(bool isValid)
    {
        SetColor(isValid ? Color.green : Color.red);
    }
    
    // <summary>
    // Optional: clear all overrides (rarely needed)
    // </summary>
    public void Clear()
    {
        if (renderers == null)
            return;
        
        foreach (var r in renderers)
        {
            if (!r)
                continue;
            
            r.SetPropertyBlock(null);
        }
    }
}