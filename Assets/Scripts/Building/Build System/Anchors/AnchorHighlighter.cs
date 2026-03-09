using UnityEngine;

public class AnchorHighlighter : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private void Start()
    {
        Toggle(false);
    }
    
    private void OnEnable()
    {
        BuildModeController.OnBuildModeChanged += Toggle;
    }
    
    private void OnDisable()
    {
        BuildModeController.OnBuildModeChanged -= Toggle;
    }

    private void Toggle(bool active)
    {
        if (rend) rend.enabled = active;
    }
}