using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using Bolt.Photon;
using UdpKit;
using UnityEngine;


namespace Launcher
{
    [BoltGlobalBehaviour( BoltNetworkModes.Server,"ClientLauncher")]
    public class ServerLauncherListener : Bolt.GlobalEventListener
    {
        private ServerLauncher _serverLauncher;

        private void Start()
        {
            _serverLauncher = FindObjectOfType<ServerLauncher>();
        }

        #region BoltCallbacks

        public override void BoltStartBegin()
        {
            Debug.LogWarning("Starting Bolt Server..");
            // Register any Protocol Token that are you using
            BoltNetwork.RegisterTokenClass<PhotonRoomProperties>();
        }

       
        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            { 
                Debug.LogWarning("Started Bolt Server!!");
                _serverLauncher.CreateRoom();
            }
        }
        
        public override void BoltShutdownBegin(AddCallback registerDoneCallback,
            UdpConnectionDisconnectReason disconnectReason)
        {
            Debug.LogWarning("Bolt server is shutting down...");

            registerDoneCallback(() =>
            {
                Debug.LogWarning("Bolt is shutdown done for server. Starting as client...");
                BoltLauncher.StartClient();
            });
        }

        

        #endregion
    }
}

