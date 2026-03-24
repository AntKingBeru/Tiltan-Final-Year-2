using UnityEngine;

public class SaveMenuController : MonoBehaviour
{
    [SerializeField] private SaveSlotUI slot1;
    [SerializeField] private SaveSlotUI slot2;

    private void Start()
    {
        slot1.Initialize(0);
        slot2.Initialize(1);
    }
}