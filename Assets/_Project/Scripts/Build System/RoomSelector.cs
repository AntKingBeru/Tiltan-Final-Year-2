using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSelector : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) return;
        
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (!Physics.Raycast(ray, out var hit)) return;
        
        var room = hit.collider.GetComponentInParent<Room>();

        if (!room) return;

        if (!room.HasUpgrade()) return;

        BuildManager.Instance.UpgradeRoom(room, room.GetUpgrade(0));
    }
}