using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetreyalBoard 
{
    public enum ExitDirection
    {
        Forward,
        Left,
        Right,
        Back,
        EnteranceExit
    }
    
    public Transform Board;
    public int MapSize = 32;
    
    public GameEquipments currEquipments;
    protected GridMapGenerator _grid;
    
    protected List<Room> _openedRooms;
    

    public BetreyalBoard()
    {
        _openedRooms = new List<Room>();
        _grid = new GridMapGenerator(MapSize,new Vector3(50f,0f,50f));
        
        currEquipments = Resources.Load<GameEquipments>("Scriptables/BuildingBlocks");
        currEquipments.SetEquipments();
    }

    public void SetBoard()
    {
        Board = new GameObject("Board").transform;
        _openedRooms.AddRange(Object.FindObjectsOfType<Room>());
    }
    
    #region ExplorerManagement
    
    #endregion

    #region RoomManagement

    public void AddRoom(int roomId,Vector2 gridPos)
    {
        Vector3 roomPos = new Vector3(_grid.gridMatrix[(int)gridPos.x,(int)gridPos.y].gridPos.x,0f,_grid.gridMatrix[(int)gridPos.x,(int)gridPos.y].gridPos.y);
        Room newRoom = UnityEngine.Object.Instantiate(currEquipments.GetRoom(roomId),roomPos,Quaternion.identity,Board);
        
        newRoom.GridPos = gridPos;
        _openedRooms.Add(newRoom);
        
        currEquipments.RemoveRoom(roomId);

        _grid.gridMatrix[(int) gridPos.x, (int) gridPos.y].occupied = true;
    }

    public bool GridOccupied(Vector2 gridPos)
    {
        if (_grid.gridMatrix[(int)gridPos.x, (int)gridPos.y].occupied)
        {
            return true;
        }
        return false;
    }

    public Room GetRoom(Vector2 gridPos)
    {
        return _openedRooms.Find(x => x.GridPos == gridPos);
    }
    
    #endregion
}
