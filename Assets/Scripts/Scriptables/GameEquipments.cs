using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameEquipment List", menuName = "Board/GameEquipment", order = 1)]
public class GameEquipments : ScriptableObject
{
    public int roomCountMultiplier;
    [Tooltip("The directory the rooms are located `ex: Prefabs/Rooms`")]
    public string roomDirectory = "Prefabs/Rooms";
    [Tooltip("The directory the interactables are located `ex: Prefabs/Interactable`")]
    public string interactableDirectory = "Scriptables/Interactables";
    [Tooltip("The directory the characters are located `ex: Prefabs/Explorers`")]
    public string characterDirectory = "Scriptables/Explorers";
    
    private List<Room> _availableRooms;
    private List<InteractableItem> _allInteractables;
    private List<ExplorerSlate_SO> _slates;
    private List<Texture2D> _cursors;

    public void SetEquipments()
    {
        _availableRooms = new List<Room>();
        for (int i = 0; i < roomCountMultiplier; i++)
        {
            _availableRooms.AddRange(Resources.LoadAll<Room>(roomDirectory));
        }
        
        _allInteractables = new List<InteractableItem>();
        _allInteractables.AddRange(Resources.LoadAll<InteractableItem>(interactableDirectory));
        
        _slates = new List<ExplorerSlate_SO>();
        _slates.AddRange(Resources.LoadAll<ExplorerSlate_SO>(characterDirectory));
        
        _cursors = new List<Texture2D>();
        _cursors.AddRange(Resources.LoadAll<Texture2D>("Textures/Cursors"));
    }

    #region Slate

    public ExplorerSlate_SO GetExplorerSlate(int id)
    {
        return _slates[id];
    }
    
    #endregion

    #region Cursor

    public Texture2D GetCursor(string name)
    {
        return _cursors.Find(x => x.name.ToLower().Contains(name));
    }

    #endregion
    
    #region Room

    public int GetEnteranceId()
    {
        for (int i = 0; i < _availableRooms.Count; i++)
        {
            if (_availableRooms[i].name.ToLower().Contains("enterance"))
                return i;
        }

        return -1;
    }
    
    public int GetRandomRoomId()
    { 
        int roomIndex = Random.Range(0, _availableRooms.Count - 1);
        return roomIndex;
    }

    public Room GetRoom(int index)
    {
        if (index >= _availableRooms.Count)
        {
            Debug.LogError("requested room not exist");
        }
        
        Room room = _availableRooms[index];

        return room;
    }

    public void RemoveRoom(int roomIndex)
    {
        _availableRooms.RemoveAt(roomIndex);
    }

    #endregion
}
