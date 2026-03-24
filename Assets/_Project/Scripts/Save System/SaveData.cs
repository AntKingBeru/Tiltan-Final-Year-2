using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string saveName;
    public long timestamp;

    public ResourceData resources;
    public GridData grid;
    public List<RoomData> rooms = new();
    public List<TrapData> traps = new();
    public List<MinionData> minions = new();
    public ProgressData progress;
}

[Serializable]
public class ResourceData
{
    public int stone;
    public int wood;
}

[Serializable]
public class GridData
{
    public List<CellData> cells = new();
}

[Serializable]
public class CellData
{
    public int x;
    public int y;
    public int cellType;
}

[Serializable]
public class RoomData
{
    public string id;
    public int x;
    public int y;
    public int width;
    public int height;
    public int rotation;
    public int upgradeLevel;
}

[Serializable]
public class TrapData
{
    public string id;
    public Vector3 position;
    public Vector3 rotation;
    public int upgradeLevel;
}

[Serializable]
public class MinionData
{
    public Vector3 position;
    public float health;
    public int state;
}

[Serializable]
public class ProgressData
{
    public int currentWave;
}