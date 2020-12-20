using System;
using System.Collections;
using System.Collections.Generic;
using ExplorerClient;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public int ExitId = -1;
    public BetreyalBoard.ExitDirection ExitDirection;

    [SerializeField]
    private Renderer glowingRenderer;
    
    private Material _glowingTriggerMat;

    private bool _exitEnterable;
    private bool _exitEntered;

    public bool Used => _exitEntered && _exitEnterable;
    
    private void OnEnable()
    {
        if (!glowingRenderer)
        {
            Debug.LogError("Assign a glowing renderer for door");
        }
        else
        {
            _glowingTriggerMat = glowingRenderer.material;
        }

        SetExitDirection();
        
        if (ExitDirection != BetreyalBoard.ExitDirection.Back)
        {
            ExitClosed();
        }
        else
        {
            ExitUsed();
        }
    }

    public event Action<int,int> enteredDoorTrigger;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Client entered exit " + ExitDirection.ToString());
        if (enteredDoorTrigger != null && ExitId >= 0)
        {
            ExplorerController triggeredExplorer = other.GetComponent<ExplorerController>();
            if(triggeredExplorer && _exitEnterable)
                enteredDoorTrigger(this.ExitId,triggeredExplorer.state.Id);
        }
    }

    public void ExitOpened()
    {
        _exitEnterable = true;
        if(_glowingTriggerMat)
            _glowingTriggerMat.SetColor("_EmissionColor", Color.yellow);
    }

    public void ExitClosed()
    {
        _exitEnterable = false;
        if(_glowingTriggerMat)
            _glowingTriggerMat.SetColor("_EmissionColor", Color.red);
    }

    public void ExitUsed()
    {
        _exitEnterable = true;
        _exitEntered = true;
        if (_glowingTriggerMat)
        {
            _glowingTriggerMat.SetColor("_EmissionColor", Color.black * 0.5f);
            
        }
    }
    
    //does it break when rotating rooms ??
    void SetExitDirection()
    {
        //if setted as enterance from inspector send this will not change exit direction
        if(ExitDirection == BetreyalBoard.ExitDirection.EnteranceExit)
            return;

        if (Mathf.Approximately(transform.forward.z, 1f))
        {
            ExitDirection = BetreyalBoard.ExitDirection.Forward;
        }
        else if (Mathf.Approximately(transform.forward.z, -1f))
        {
            ExitDirection = BetreyalBoard.ExitDirection.Back;
        }
        else if (Mathf.Approximately(transform.forward.x, 1f))
        {
            ExitDirection = BetreyalBoard.ExitDirection.Right; 
        }
        else if (Mathf.Approximately(transform.forward.x, -1f))
        {
            ExitDirection = BetreyalBoard.ExitDirection.Left;
        }
       // Debug.LogWarning(transform.forward);
    }
}
