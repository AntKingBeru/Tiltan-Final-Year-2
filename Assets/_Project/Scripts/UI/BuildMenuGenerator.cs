using UnityEngine;

public class BuildMenuGenerator : MonoBehaviour
{
    [Header("Blueprint Lists")]
    [SerializeField] private RoomBlueprint[] roomBlueprints;
    [SerializeField] private TrapBlueprint[] trapBlueprints;
    
    [Header("UI")]
    [SerializeField] private Transform roomsParent;
    [SerializeField] private Transform trapsParent;
    
    [SerializeField] private BuildButton buttonPrefab;

    private void Start()
    {
        GenerateRooms();
        GenerateTraps();
    }

    private void GenerateRooms()
    {
        foreach (var room in roomBlueprints)
        {
            var button = Instantiate(buttonPrefab, roomsParent);
            button.Setup(room);
        }
    }
    
    private void GenerateTraps()
    {
        foreach (var trap in trapBlueprints)
        {
            var button = Instantiate(buttonPrefab, trapsParent);
            button.Setup(trap);
        }
    }
}