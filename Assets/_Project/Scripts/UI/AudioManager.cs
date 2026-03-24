using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        LoadVolumes();
    }
    
    #region Public API

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat(SettingsKeys.MasterVolume, ToDecibel(value));
        PlayerPrefs.SetFloat(SettingsKeys.MasterVolume, value);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(SettingsKeys.MusicVolume, ToDecibel(value));
        PlayerPrefs.SetFloat(SettingsKeys.MusicVolume, value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SettingsKeys.SFXVolume, ToDecibel(value));
        PlayerPrefs.SetFloat(SettingsKeys.SFXVolume, value);
    }
    
    #endregion
    
    #region Load

    private void LoadVolumes()
    {
        var master = PlayerPrefs.GetFloat(SettingsKeys.MasterVolume, 1f);
        var music = PlayerPrefs.GetFloat(SettingsKeys.MusicVolume, 1f);
        var sfx = PlayerPrefs.GetFloat(SettingsKeys.SFXVolume, 1f);
        
        ApplyVolumes(master, music, sfx);
    }

    private void ApplyVolumes(float master, float music, float sfx)
    {
        mixer.SetFloat(SettingsKeys.MasterVolume, ToDecibel(master));
        mixer.SetFloat(SettingsKeys.MusicVolume, ToDecibel(music));
        mixer.SetFloat(SettingsKeys.SFXVolume, ToDecibel(sfx));
    }
    
    #endregion

    private float ToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }
}