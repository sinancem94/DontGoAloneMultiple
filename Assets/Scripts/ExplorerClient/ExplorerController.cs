using Bolt;
using ExplorerClient.ExplorerUI;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace ExplorerClient
{
    public class ExplorerController : Bolt.EntityEventListener<IExplorerState>
    {
        private bool _gridPosChanged = false;
        private bool _enteredRoom = false;
    
        private NavMeshAgent _navigator;
        private Vector3 _clickedPos;

        private bool _forcedMove;
        
        private void Start()
        {
            //if neither controller or server transform will be controlled by bolt only
            if (entity.IsControllerOrOwner == false )
            {
                if(GetComponent<NavMeshAgent>())
                    GetComponent<NavMeshAgent>().enabled = false;
                if (GetComponent<LocalNavMeshBuilder>())
                    GetComponent<LocalNavMeshBuilder>().enabled = false;
            }
        }
    
        public override void Attached()
        {
            base.Attached();
            ExplorerGameManager.instance.explorer = this;

            state.SetTransforms(state.Transform, transform);
            _navigator = GetComponent<NavMeshAgent>();
        
            _clickedPos = transform.position;

            state.AddCallback("GridPos",ExplorerGridPosChanged);

            //set tag name
            TextMeshPro tagName =  this.GetComponentInChildren<TextMeshPro>();
            if(tagName.gameObject.GetComponent<TextLookAtCamera>() == null)
                tagName.gameObject.AddComponent<TextLookAtCamera>();
            tagName.text = state.NickName;
        }

        public override void Detached()
        {
            base.Detached();
        }

        public override void SimulateController()
        {
            if (_gridPosChanged)
            {
                if(!ExplorerGameManager.instance.board.GetRoom(state.GridPos)) //wait until room has created on client
                    return;
            
                _gridPosChanged = false;
                Vector3 newPos =  ExplorerGameManager.instance.board.GetRoom(state.GridPos).transform.position;
                newPos.y = transform.position.y;
                ExplorerCamera.instance.BeamCamera(newPos);

                _enteredRoom = true;
                _clickedPos = newPos;
            }
        
            IClickToMoveCommandInput input = ClickToMoveCommand.Create();
            input.click = _clickedPos;
            entity.QueueInput(input);
        }
    
        public override void ExecuteCommand(Command command, bool resetState)
        {
            ClickToMoveCommand cmd = (ClickToMoveCommand)command;

            if (resetState)
            {
                NavMeshPath path = new NavMeshPath();
            
                if(_navigator.CalculatePath(cmd.Result.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    _navigator.SetPath(path);
                else
                {
                    _navigator.Warp(cmd.Result.position);
                    _navigator.ResetPath();
                }
            }
            else
            {
                NavMeshPath path = new NavMeshPath();
                
                if (_enteredRoom)
                {
                    _enteredRoom = false;
                    _navigator.Warp(cmd.Input.click);
                    _navigator.ResetPath();
                }
                else
                {
                    if (_forcedMove)
                    {
                        _navigator.SetDestination(cmd.Input.click);
                    }
                    else if(_navigator.CalculatePath(cmd.Input.click, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        // Debug.LogWarning(path.status);
                        _navigator.SetPath(path);
                    }
                }
                  
                cmd.Result.position = transform.position;
            }
        }

        #region StateChangeCallbacks

        //explorer entered new room
        void ExplorerGridPosChanged()
        {
            //ClientGameManager.instance.clientBoard.EnterRoom(state.GridPos);
            _gridPosChanged = true;
        }

        #endregion
   
        #region public methods

        public void MoveTo(Vector3 clickPos, bool force = false)
        {
            _clickedPos = clickPos;
            _clickedPos.y = transform.position.y;

            _forcedMove = force;

        }

        #endregion
        
    }
}
