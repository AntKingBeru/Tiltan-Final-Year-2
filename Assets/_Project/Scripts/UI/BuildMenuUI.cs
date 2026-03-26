using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [Header("Mode Indicators")]
    [SerializeField] private Image buildImage;
    [SerializeField] private Image clearImage;
    [SerializeField] private Image upgradeImage;
    
    [Header("Prefabs")]
    [SerializeField] private BuildButtonUI buttonPrefab;

    [Header("Parents")]
    [SerializeField] private GameObject tabsContainer;
    [SerializeField] private Transform roomsParent;
    [SerializeField] private Transform trapsParent;
    
    [Header("Data")]
    [SerializeField] private RoomBlueprint[] rooms;
    [SerializeField] private TrapBlueprint[] traps;

    private readonly Color _inactiveColor = new Color(0.2f, 0.2f, 0.2f);
    private readonly Color _activeColor = new Color(0.1f, 0.5f, 0.1f);
    
    private bool _showingRooms = true;

    private void Start()
    {
        GenerateRooms();
        GenerateTraps();
        
        ShowRooms();

        UpdateModeUI(BuildManager.Instance.CurrentMode);
    }

    private void OnEnable()
    {
        if (BuildManager.Instance)
            BuildManager.Instance.OnModeChanged += UpdateModeUI;
    }
    
    private void OnDisable()
    {
        if (BuildManager.Instance)
            BuildManager.Instance.OnModeChanged -= UpdateModeUI;
    }

    private void GenerateRooms()
    {
        foreach (var room in rooms)
        {
            var btn = Instantiate(buttonPrefab, roomsParent);
            btn.Setup(room);
        }
    }
    
    private void GenerateTraps()
    {
        foreach (var trap in traps)
        {
            var btn = Instantiate(buttonPrefab, trapsParent);
            btn.Setup(trap);
        }
    }

    public void ShowRooms()
    {
        if (!BuildManager.Instance.IsBuildMode)
            return;

        _showingRooms = true;
        
        roomsParent.gameObject.SetActive(true);
        trapsParent.gameObject.SetActive(false);
    }
    
    public void ShowTraps()
    {
        if (!BuildManager.Instance.IsBuildMode)
            return;
        
        _showingRooms = false;
        
        trapsParent.gameObject.SetActive(true);
        roomsParent.gameObject.SetActive(false);
    }

    private void UpdateModeUI(BuildMode mode)
    {
        SetImage(buildImage, false);
        SetImage(clearImage, false);
        SetImage(upgradeImage, false);

        if (tabsContainer)
            tabsContainer.SetActive(false);
        
        roomsParent.gameObject.SetActive(false);
        trapsParent.gameObject.SetActive(false);

        switch (mode)
        {
            case BuildMode.Build:
                SetImage(buildImage, true);
                
                if (_showingRooms)
                    ShowRooms();
                else 
                    ShowTraps();
                
                if (tabsContainer)
                    tabsContainer.SetActive(true);
                
                break;

            case BuildMode.Clear:
                SetImage(clearImage, true);
                break;

            case BuildMode.Upgrade:
                SetImage(upgradeImage, true);
                break;

            case BuildMode.None:
            default:
                break;
        }
    }

    private void SetImage(Image img, bool active)
    {
        if (!img)
            return;
        
        img.color = active ? _activeColor : _inactiveColor;
    }
}