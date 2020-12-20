using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bolt.AdvancedTutorial;
using ExplorerClient;
using UnityEngine;

public class Room : MonoBehaviour
{
    private List<RoomExit> _exits;
    private Vector2 _gridPos = Vector2.one * -1;

    public Vector2 GridPos
    {
        get => _gridPos;
        set => _gridPos = value;
    }
    
    private void OnEnable()
    {
        _exits = new List<RoomExit>();
        int exitIdIncrementer = 0;
        foreach (var door in GetComponentsInChildren<RoomExit>())
        {
            door.ExitId = exitIdIncrementer;
            door.enteredDoorTrigger += ExitEntered; //add trigger event for exiting from door
            
            _exits.Add(door);
            //Debug.LogWarning("aldim arttirdim gardass id : " + door.ExitId);
            exitIdIncrementer++;
        }
    }

    public void AllExitsEnterable()
    {
        foreach (var roomExit in _exits)
        {
            if(roomExit.Used == false)
                roomExit.ExitOpened();
        }
    }
    
    protected virtual void ExitEntered(int exitId,int explorerId)
    {
        if (BoltNetwork.IsClient || ServerGameManager.instance.serverPlayer.State.Id == explorerId)
        {
            ExplorerGameManager.instance.AskQuestionToExplorer(ExplorerQuestionManager.QuestionType.OpenDoor);
        }

        if(BoltNetwork.IsClient) return;
        
        BetreyalBoard.ExitDirection direction = _exits[exitId].ExitDirection;
        Debug.Log("Client entered exit " + direction);
        Vector2 newRoomGridPos = _gridPos;
        
        switch (direction)
        {
            case  BetreyalBoard.ExitDirection.Forward: 
                newRoomGridPos = new Vector2(_gridPos.x + 1,_gridPos.y);
                break;
            case  BetreyalBoard.ExitDirection.Back: 
                newRoomGridPos = new Vector2(_gridPos.x - 1,_gridPos.y);
                break;
            case BetreyalBoard.ExitDirection.Left:
                newRoomGridPos = new Vector2(_gridPos.x,_gridPos.y - 1);
                break;
            case BetreyalBoard.ExitDirection.Right:
                newRoomGridPos = new Vector2(_gridPos.x,_gridPos.y + 1);
                break;
            case BetreyalBoard.ExitDirection.EnteranceExit:
                Debug.Log("EnteranceExit ");
                newRoomGridPos = ServerGameManager.instance.initialRoomGridPos;
                break;
            default:
                break;
        }

        ServerGameManager.instance.ExplorerEnteredExit(explorerId, newRoomGridPos);
    }

    
}
