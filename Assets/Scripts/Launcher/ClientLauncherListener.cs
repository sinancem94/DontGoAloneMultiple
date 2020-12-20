using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;
using Bolt.Photon;
using TMPro;
using UdpKit.Platform.Photon;
using UdpKit;
using UnityEngine.UI;

namespace Launcher
{
    [BoltGlobalBehaviour( BoltNetworkModes.Client,"ClientLauncher")]
    public class ClientLauncherListener :  Bolt.GlobalEventListener
    {
        private ClientLauncher _launcher;
  
        private void Start()
        { 
            _launcher = FindObjectOfType<ClientLauncher>();
        }
    
        #region BoltCallbacks

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            _launcher.ListUpdated(sessionList);
        }

        public override void BoltStartBegin()
        {
            // Register any Protocol Token that are you using
            BoltNetwork.RegisterTokenClass<PhotonRoomProperties>();
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback,
            UdpConnectionDisconnectReason disconnectReason)
        {
            Debug.LogWarning("Bolt is shutting down...");

            registerDoneCallback(() =>
            {
                Debug.LogWarning("Bolt is shutdown done for client.");
            });
        }

        #endregion 
  
    }
}


