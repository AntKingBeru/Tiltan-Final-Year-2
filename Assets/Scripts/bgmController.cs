using UnityEngine;

public class bgmController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    public void ToggleMute()
    {
      audioSource.mute = !audioSource.mute;
    }

}
