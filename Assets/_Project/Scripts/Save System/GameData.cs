using System.Collections.Generic;

[System.Serializable]
public class SavedCell
{
    public int x, y;
}

[System.Serializable]
public class SavedRoom
{
    public string blueprintName;
    public int gridX, gridY;
}

[System.Serializable]
public class SavedTrap
{
    public int roomGridX, roomGridY;
    public int anchorIndex;
    public string blueprintName;
}

[System.Serializable]
public class GameData
{
    // Settings
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public int difficulty;

    // Resources
    public int stone;
    public int wood;

    // Grid state - only cleared cells, not the full 50x50 grid
    public List<SavedCell> clearedCells;

    // Placements
    public List<SavedRoom> rooms;
    public List<SavedTrap> traps;

    // Progress - ready for wave/score system
    public int currentWave;
    public int score;

    public GameData()
    {
        masterVolume = 1f;
        musicVolume = 1f;
        sfxVolume = 1f;
        difficulty = 0;

        stone = 0;
        wood = 0;

        clearedCells = new List<SavedCell>();
        rooms = new List<SavedRoom>();
        traps = new List<SavedTrap>();

        currentWave = 0;
        score = 0;
    }
}
