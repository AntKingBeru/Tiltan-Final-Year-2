using UnityEngine;
using System.Collections.Generic;

public class RoomRegistry : MonoBehaviour
{
    public static RoomRegistry Instance { get; private set; }
    
    private readonly List<Room> _rooms = new();

    private void Awake()
    {
        Instance = this;
    }
    
    public void Register(Room room)
    {
        if (!_rooms.Contains(room))
            _rooms.Add(room);
    }
    
    public void Unregister(Room room)
    {
        _rooms.Remove(room);
    }
    
    public List<Room> GetAllRooms() => _rooms;   
}