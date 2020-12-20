using System;
using System.Collections;
using System.Collections.Generic;
using UdpKit.Platform.Photon;
using UnityEngine;

namespace Launcher
{
    public class SessionHolder : MonoBehaviour
    {
        public string hostName;
        public int id;
    
        private PhotonSession _session;
        private string _sessionScene;
    
        public PhotonSession Session
        {
            get => _session;
        }

        public string Scene
        {
            get => _sessionScene;
        }
    
        public void SetSession(PhotonSession photonSession)
        {
            this._session = photonSession;
            hostName = _session.HostName;

            object sceneName;
            if (photonSession.Properties.TryGetValue("m", out sceneName))
            {
                _sessionScene = sceneName as string;
            }

        }

        #region UIButtonCallbacks

        public event Action<SessionHolder> selectedSession; 
        public void OnSelectSession()
        {
            if (selectedSession != null)
            {
                selectedSession(this);
            }
        }
        

        #endregion
    }
}


