using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    private const string MainMenuScene = "Main Menu";

    [SerializeField] private GameObject panel;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private UnityEngine.UI.Button saveButton;

    private void Start()
    {
        panel.SetActive(false);
        if (saveButton != null) saveButton.onClick.AddListener(SaveGame);

        var master = PlayerPrefs.GetFloat(SettingsKeys.MasterVolume, 1f);
        var music = PlayerPrefs.GetFloat(SettingsKeys.MusicVolume, 1f);
        var sfx = PlayerPrefs.GetFloat(SettingsKeys.SFXVolume, 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (panel.activeSelf)
                Close();
            else
                Open();
        }

        if (Keyboard.current.f5Key.wasPressedThisFrame)
            SaveGame();
    }

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(MainMenuScene);
    }

    public void SaveGame()
    {
        if (SaveSystem.Instance == null) { Debug.LogError("SaveSystem is null!"); return; }
        if (WaveManager.Instance == null) { Debug.LogError("WaveManager is null!"); return; }

        var slot = SaveSystem.Instance.ActiveSlot;
        SaveSystem.Instance.Save(slot, $"Wave {WaveManager.Instance.CurrentWave}");
        Debug.Log($"Game saved to slot {slot}");
    }
}