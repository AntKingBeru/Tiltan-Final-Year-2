using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private const string MainMenuScene = "Main Menu";

    [SerializeField] private GameObject panel;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
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
}