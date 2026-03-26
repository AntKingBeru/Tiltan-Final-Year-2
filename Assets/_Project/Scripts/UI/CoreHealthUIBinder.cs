using UnityEngine;

public class CoreHealthUIBinder : MonoBehaviour
{
    public static CoreHealthUIBinder Instance { get; private set; }

    [SerializeField] private CoreHealthUI healthUI;

    private void Awake()
    {
        Instance = this;
    }

    public void Bind(Core core)
    {
        healthUI.Bind(core);
    }
}