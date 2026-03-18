using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [SerializeField] private string _saveFileName = "Save.json";

    private GameData _gameData;
    private FileDataHandler _dataHandler;

    // Registration list - objects add/remove themselves, no FindObjectsOfType scanning
    private readonly List<IDataPersistence> _dataPersistences = new List<IDataPersistence>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DataPersistenceManagers found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _saveFileName);
    }

    #region Registration

    public void Register(IDataPersistence dataPersistence)
    {
        if (!_dataPersistences.Contains(dataPersistence))
            _dataPersistences.Add(dataPersistence);
    }

    public void Unregister(IDataPersistence dataPersistence)
    {
        _dataPersistences.Remove(dataPersistence);
    }

    #endregion

    #region Save & Load

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if (_gameData == null)
        {
            Debug.Log("No save file found. Starting new game.");
            NewGame();
        }

        foreach (IDataPersistence dp in _dataPersistences)
            dp.LoadData(_gameData);
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            Debug.LogWarning("SaveGame called but no GameData exists. Call NewGame or LoadGame first.");
            return;
        }

        foreach (IDataPersistence dp in _dataPersistences)
            dp.SaveData(ref _gameData);

        _dataHandler.Save(_gameData);
    }

    #endregion

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
