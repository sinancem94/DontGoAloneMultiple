using Bolt;
using UnityEngine;

namespace ExplorerClient
{
    [BoltGlobalBehaviour("BuildingBlocks")]
    public class ExplorerNetworkCallbacks : Bolt.GlobalEventListener
    {
        // private bool started = false;

        public override void SceneLoadLocalBegin(string scene, IProtocolToken token)
        {
            //Debug.LogError("SceneLoadLocalBegin explorer");
       
            if(BoltNetwork.IsClient)
                BoltNetwork.LoadSceneSync();
       
            ExplorerGameManager.Instantiate();
            ExplorerGameManager.instance.board = new BetreyalBoard();
        }

        public override void SceneLoadLocalDone(string scene, IProtocolToken token)
        {
            ExplorerGameManager.instance.board.SetBoard();
            ExplorerCamera.Instantiate();
        }

        public override void ControlOfEntityGained(BoltEntity entity)
        {
            if(entity.StateIs<IExplorerState>())
                ExplorerGameManager.instance.SetManager();
        }


        #region CustomEvents

        public override void OnEvent(AddRoomEvent evnt)
        {
            base.OnEvent(evnt);
            ExplorerGameManager.instance.board.AddRoom(evnt.roomId,evnt.gridId);
            //ExplorerGameManager.instance.explorer.state.GridPos = evnt.gridId;
        }

        public override void OnEvent(StartGameEvent evnt)
        {
            Room enterance = ExplorerGameManager.instance.board.GetRoom(Vector2.one * -1);
        
            if (enterance)
            {
                enterance.AllExitsEnterable();
            }
       
        }

        #endregion
    
    }
}
