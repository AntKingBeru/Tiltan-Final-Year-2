using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private Image screenshot;
    [SerializeField] private TMP_Text label;

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
        if (SaveSystem.Instance.HasSave(_slotIndex))
        {
            var data = SaveSystem.Instance.Load(_slotIndex);
            SaveSystem.Instance.ApplySave(data);
        }
        else
        {
            SaveSystem.Instance.Save(_slotIndex, $"Save {_slotIndex}");
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