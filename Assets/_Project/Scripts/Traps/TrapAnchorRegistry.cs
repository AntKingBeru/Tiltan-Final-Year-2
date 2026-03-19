using UnityEngine;
using System.Collections.Generic;

public class TrapAnchorRegistry : MonoBehaviour
{
    public static TrapAnchorRegistry Instance { get; private set; }
    
    private readonly List<TrapAnchor> _anchors = new();

    private void Awake()
    {
        Instance = this;
    }
    
    public void Register(TrapAnchor anchor)
    {
        if (!_anchors.Contains(anchor))
            _anchors.Add(anchor);
    }
    
    public void Unregister(TrapAnchor anchor)
    {
        _anchors.Remove(anchor);
    }

    public List<TrapAnchor> GetAllAnchors() => _anchors;
}