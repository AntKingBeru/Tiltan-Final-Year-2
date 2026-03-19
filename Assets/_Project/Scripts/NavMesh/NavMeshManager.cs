using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance { get; private set; }

    [SerializeField] private NavMeshSurface surface;

    private void Awake()
    {
        Instance = this;
    }

    public void Rebuild()
    {
        surface.BuildNavMesh();
    }
}