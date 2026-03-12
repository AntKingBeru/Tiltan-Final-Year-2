using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfoPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image icon;

    private void Start()
    {
        // BuildManager.Instance.OnBlueprintSelected += UpdatePanel;
    }

    private void OnDestroy()
    {
        // BuildManager.Instance.OnBlueprintSelected -= UpdatePanel;
    }

    private void UpdatePanel(RoomBlueprint blueprint)
    {
        nameText.text = blueprint.roomName;
        descriptionText.text = blueprint.description;
        
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
        
        icon.sprite = blueprint.icon;
    }
    
    private void UpdatePanel(TrapBlueprint blueprint)
    {
        nameText.text = blueprint.trapName;
        descriptionText.text = blueprint.description;
        
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
        
        icon.sprite = blueprint.icon;
    }
}