using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private Image screenshot;
    [SerializeField] private TMP_Text label;
    [SerializeField] private bool isMainMenuMode = false;

    private const string GameSceneName = "GameScene";
    private int _slotIndex;

    public void Initialize(int slot)
    {
        _slotIndex = slot;

        if (SaveSystem.Instance.HasSave(slot))
        {
            var data = SaveSystem.Instance.Load(slot);
            label.text = data.saveName;

            LoadScreenshot(slot);
        }
        else
        {
            label.text = "Empty Slot";
            screenshot.color = Color.black;
        }
    }

    public void OnClick()
    {
        if (isMainMenuMode)
        {
            if (SaveSystem.Instance.HasSave(_slotIndex))
                SaveSystem.Instance.SetPendingLoad(_slotIndex);
            else
                SaveSystem.Instance.SetActiveSlot(_slotIndex);

            SceneLoader.Instance.LoadScene(GameSceneName);
        }
        else
        {
            // In-game: save current state to this slot
            SaveSystem.Instance.Save(_slotIndex, $"Wave {WaveManager.Instance.CurrentWave}");
            Initialize(_slotIndex); // Refresh the UI to show new save
        }
    }

    private void LoadScreenshot(int slot)
    {
        var path = Path.Combine(Application.persistentDataPath, $"slot_{slot}.png");

        if (!File.Exists(path))
            return;
        
        var bytes = File.ReadAllBytes(path);
        
        var texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        screenshot.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            Vector2.one * 0.5f
        );
    }
}