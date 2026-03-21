using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text costText;

    private RoomBlueprint _room;
    private TrapBlueprint _trap;

    public void Setup(RoomBlueprint room)
    {
        _room = room;
        _trap = null;

        label.text = room.name;
        costText.text = $"S:{room.stoneCost} W:{room.woodCost}";

        button.onClick.AddListener(() =>
        {
            BuildManager.Instance.SelectRoom(_room);
        });
        
        button.interactable = ResourceManager.Instance.CanAfford(room.stoneCost, room.woodCost);
    }
    
    public void Setup(TrapBlueprint trap)
    {
        _room = null;
        _trap = trap;
        
        label.text = trap.name;
        costText.text = $"S:{trap.stoneCost} W:{trap.woodCost}";
        
        button.onClick.AddListener(() =>
        {
            BuildManager.Instance.SelectTrap(_trap);
        });
        
        button.interactable = ResourceManager.Instance.CanAfford(trap.stoneCost, trap.woodCost);
    }
}