using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButtonUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button button;
    
    private RoomBlueprint _roomBlueprint;
    private TrapBlueprint _trapBlueprint;

    public void InitializeRoom(RoomBlueprint blueprint)
    {
        _roomBlueprint = blueprint;

        icon.sprite = blueprint.icon;
        
        if (blueprint.stoneCost != 0)
        {
            costText.text = blueprint.woodCost != 0 ? $"{blueprint.stoneCost} Stone/ {blueprint.woodCost} Wood" : $"{blueprint.stoneCost} Stone";
        }
        else
        {
            if (blueprint.woodCost != 0)
            {
                costText.text = $"{blueprint.woodCost} Wood";
            }
        }
        
        button.onClick.AddListener(OnClickedRoom);
    }
    
    public void InitializeTrap(TrapBlueprint blueprint)
    {
        _trapBlueprint = blueprint;

        icon.sprite = blueprint.icon;
        
        if (blueprint.stoneCost != 0)
        {
            costText.text = blueprint.woodCost != 0 ? $"{blueprint.stoneCost} Stone/ {blueprint.woodCost} Wood" : $"{blueprint.stoneCost} Stone";
        }
        else
        {
            if (blueprint.woodCost != 0)
            {
                costText.text = $"{blueprint.woodCost} Wood";
            }
        }
        
        button.onClick.AddListener(OnClickedTrap);
    }

    private void OnClickedRoom()
    {
        BuildManager.Instance.SelectRoom(_roomBlueprint);
    }

    private void OnClickedTrap()
    {
        BuildManager.Instance.SelectTrap(_trapBlueprint);
    }
}