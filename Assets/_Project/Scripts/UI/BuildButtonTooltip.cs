using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RoomBlueprint _room;
    private TrapBlueprint _trap;

    public void Initialize(RoomBlueprint room)
    {
        _room = room;
    }

    private void Initialize(TrapBlueprint trap)
    {
        _trap = trap;   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_room) TooltipUI.Instance.Show(_room.roomName, _room.description );
        
        if (_trap) TooltipUI.Instance.Show(_trap.trapName, _trap.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }
}