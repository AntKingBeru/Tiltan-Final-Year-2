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

    private void OnEnable()
    {
        if (ResourceManager.Instance)
            ResourceManager.Instance.OnResourcesChanged += UpdateInteractable;
        UpdateInteractable();
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance)
            ResourceManager.Instance.OnResourcesChanged -= UpdateInteractable;
    }

    public void Setup(RoomBlueprint room)
    {
        _room = room;
        _trap = null;

        label.text = room.name;
        costText.text = $"S:{room.stoneCost} W:{room.woodCost}";

        SetUpButton();
        UpdateInteractable();
    }
    
    public void Setup(TrapBlueprint trap)
    {
        _room = null;
        _trap = trap;
        
        label.text = trap.name;
        costText.text = $"S:{trap.stoneCost} W:{trap.woodCost}";
        
        SetUpButton();
        UpdateInteractable();
    }

    private void SetUpButton()
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            if (_room)
                BuildManager.Instance.SelectRoom(_room);
            else if (_trap)
                BuildManager.Instance.SelectTrap(_trap);
        });
    }

    private void UpdateInteractable()
    {
        if (!ResourceManager.Instance)
            return;

        var canAfford = false;
        
        if (_room)
            canAfford = ResourceManager.Instance.CanAfford(_room.stoneCost, _room.woodCost);
        else if (_trap)
            canAfford = ResourceManager.Instance.CanAfford(_trap.stoneCost, _trap.woodCost);
        
        button.interactable = canAfford;
        
        costText.alpha = canAfford ? 1f : 0.5f;
    }
}