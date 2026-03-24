using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string LoadingScene = "LoadingScene";

    public static SceneLoader Instance { get; private set; }

    private string _targetScene;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        _targetScene = sceneName;
        SceneManager.LoadScene(LoadingScene);
    }
    
    public string GetTargetScene() => _targetScene;
}