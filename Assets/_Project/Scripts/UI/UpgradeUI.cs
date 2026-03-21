using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text infoText;
    
    private Room _selectedRoom;

    public void Show(Room room)
    {
        _selectedRoom = room;
        panel.SetActive(true);

        infoText.text = "Upgrade Room";
    }
    
    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgrade);
    }

    private void OnUpgrade()
    {
        _selectedRoom?.Upgrade();
    }
}