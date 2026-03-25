using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance { get; private set; }

    [Header("Surfaces")]
    [SerializeField] private List<NavMeshSurface> surfaces;

    [SerializeField] private float rebuildCooldown = 0.2f;
    
    private float _lastRebuildTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Rebuild();
    }
    
    #region Public API

    public void Rebuild()
    {
        if (Time.time - _lastRebuildTime < rebuildCooldown)
            return;
        
        _lastRebuildTime = Time.time;
        
        foreach (var surface in surfaces)
            surface.BuildNavMesh();
    }

    public void Rebuild(NavMeshSurface specificSurface)
    {
        if (Time.time - _lastRebuildTime < rebuildCooldown)
            return;
        
        _lastRebuildTime = Time.time;
        
        if (specificSurface)
            specificSurface.BuildNavMesh();
    }
    
    #endregion
}