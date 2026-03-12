using UnityEngine;
using TMPro;

public class ResourceDisplayUI : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private TMP_Text valueText;
    
    private ResourceManager _resourceManager;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;

        UpdateDisplay();

        // _resourceManager.OnResourceChanged += OnResourceChanged;
    }

    private void OnDestroy()
    {
        // if (_resourceManager) _resourceManager.OnResourceChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourceType type, int value)
    {
        if (type != resourceType) return;
        
        valueText.text = value.ToString();
    }

    private void UpdateDisplay()
    {
        var value = resourceType == ResourceType.Stone ? _resourceManager.Stone : _resourceManager.Wood;
        
        valueText.text = value.ToString();
    }
}