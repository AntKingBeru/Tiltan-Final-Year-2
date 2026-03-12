using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    private RoomBlueprint _roomBlueprint;
    private TrapBlueprint _trapBlueprint;

    public void Setup(RoomBlueprint blueprint)
    {
        _roomBlueprint = blueprint;
        _trapBlueprint = null;

        icon.sprite = blueprint.icon;
    }
    
    public void Setup(TrapBlueprint blueprint)
    {
        _trapBlueprint = blueprint;
        _roomBlueprint = null;
        
        icon.sprite = blueprint.icon;
    }

    public void OnClick()
    {
        if (_roomBlueprint)
        {
            BuildManager.Instance.SelectRoom(_roomBlueprint);
            return;
        }

        if (_trapBlueprint) BuildManager.Instance.SelectTrap(_trapBlueprint);
    }
}