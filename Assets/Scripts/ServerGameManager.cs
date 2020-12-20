using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using ExplorerClient;
using UnityEngine.UIElements;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "BuildingBlocks")]
public class ServerGameManager : BoltSingletonPrefab<ServerGameManager>
{
    public ServerBoardManager boardManager;
    public Vector2 initialRoomGridPos;
    public Explorer serverPlayer;
    
    private void Start()
    {
        //Debug.LogError("start");
        boardManager = new ServerBoardManager(ExplorerGameManager.instance.board);
        initialRoomGridPos = 
            new Vector2((int)(boardManager.board.MapSize / 2), (int)(boardManager.board.MapSize / 3));
    }

    public void StartGame()
    {
        int startingRoomIndex = boardManager.GetEnterance();
        AddRoomEvent.Post(GlobalTargets.Everyone,ReliabilityModes.ReliableOrdered ,startingRoomIndex,initialRoomGridPos,0);
        StartGameEvent.Post(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
    }

    public void ExplorerEnteredExit(int explorerId,Vector2 enteredGridPos)
    {
        bool occupied = boardManager.board.GridOccupied(enteredGridPos);
        
        if (!occupied)//generate a new room if grid is empty
        {
            int addedRoomIndex = boardManager.GetRandomRoom();
            AddRoomEvent.Post(GlobalTargets.Everyone,ReliabilityModes.ReliableOrdered ,addedRoomIndex,enteredGridPos,0);
        }

        Explorer explorer = boardManager.GetExplorer(explorerId);
        explorer.State.GridPos = enteredGridPos;
    }
}

public sealed class ServerBoardManager
{
    public BetreyalBoard board;
    private List<Explorer> _explorers;
    
    public ServerBoardManager(BetreyalBoard board)
    {
        this.board = board;
        _explorers = new List<Explorer>();
    }

    #region ExplorerManagement

    public void AddExplorer(Explorer newExplorer)
    {
        _explorers.Add(newExplorer);
    }
        
    public void RemoveExplorer(Explorer newExplorer)
    {
        _explorers.Remove(newExplorer);
    }
        
    public Explorer GetExplorer(int id)
    { 
        return _explorers.Find(x => x.entity && x.ExplorerId == id);
    }

        
    #endregion
        
    #region RoomManagement

    public int GetRandomRoom()
    {
        int roomIndex = board.currEquipments.GetRandomRoomId();
        return roomIndex;
    }

    public int GetEnterance()
    {
        return board.currEquipments.GetEnteranceId();
    }

    #endregion
}
 