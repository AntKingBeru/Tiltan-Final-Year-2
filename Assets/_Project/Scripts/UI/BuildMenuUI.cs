using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private BuildButtonUI buttonPrefab;

    [Header("Parents")]
    [SerializeField] private Transform roomsParent;
    [SerializeField] private Transform trapsParent;
    
    [Header("Data")]
    [SerializeField] private RoomBlueprint[] rooms;
    [SerializeField] private TrapBlueprint[] traps;

    private void Start()
    {
        GenerateRooms();
        GenerateTraps();
        
        ShowRooms();
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
        roomsParent.gameObject.SetActive(true);
        trapsParent.gameObject.SetActive(false);
    }
    
    public void ShowTraps()
    {
        trapsParent.gameObject.SetActive(true);
        roomsParent.gameObject.SetActive(false);
    }

    public void OnBuildClicked()
    {
        BuildManager.Instance.SetBuildMode();
    }
    
    public void OnClearClicked()
    {
        BuildManager.Instance.SetNone();
    }
    
    public void OnUpgradeClicked()
    {
        BuildManager.Instance.SetUpgradeMode();
    }
    
    public void OnCancelClicked()
    {
        BuildManager.Instance.SetNone();
    }
}