using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private InitialRoomPlacer initialRoomPlacer;

    private void Start()
    {
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasPendingLoad())
        {
            int slot = SaveSystem.Instance.ConsumePendingLoad();
            var data = SaveSystem.Instance.Load(slot);

            if (data != null)
            {
                SaveSystem.Instance.ApplySave(data);
                return;
            }
        }

        // No pending save — start a fresh new game
        initialRoomPlacer.GenerateInitialRooms();
    }
}
