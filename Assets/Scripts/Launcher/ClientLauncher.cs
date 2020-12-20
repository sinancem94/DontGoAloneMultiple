using System;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Matchmaking;
using TMPro;
using UdpKit;
using UdpKit.Platform.Photon;
using UnityEngine.UI;

namespace Launcher
{
    public class ClientLauncher : MonoBehaviour
    {
        [SerializeField]
        private GameObject createRoomPanel;
    
        private GameObject _sessionListGroup;
        private List<SessionHolder> _sessionList;
        private GameObject _sessionListElement;

        public int selectedSessionId = -1;
    
        private void Awake()
        {
            Screen.SetResolution(1500,750,false); 
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _sessionList = new List<SessionHolder>();
            _sessionListElement = Resources.Load<GameObject>("Prefabs/UI/SessionListElement");

            _sessionListGroup = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        
            BoltLauncher.StartClient();
        }

        public void ListUpdated(Map<Guid, UdpSession> sessionList)
        {
            foreach (var oldSession in _sessionList)
            {
                Destroy(oldSession.gameObject);
            }

            _sessionList.Clear();

            int idCounter = 0;
            foreach (var newSession in sessionList)
            {
                var photonSession = newSession.Value as PhotonSession;
            
                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    GameObject newListElement = Instantiate(_sessionListElement, _sessionListGroup.transform);
                    SessionHolder sessionHolder = newListElement.GetComponent<SessionHolder>();
                    sessionHolder.id = idCounter;
                    sessionHolder.SetSession(photonSession);
                   
                    var matchName = sessionHolder.Scene;
                    var label = string.Format("Join: {0} | {1}/{2}", matchName, photonSession.ConnectionsCurrent, photonSession.ConnectionsMax);
    
                    newListElement.GetComponentInChildren<TextMeshProUGUI>().text = label;

                    sessionHolder.selectedSession += OnSelectSession;
                    
                    _sessionList.Add(sessionHolder);
                    idCounter++;
                }
            }
        }


        #region UIButtonCalls

        public void OnSelectSession(SessionHolder sessionHolder)
        {
            Toggle toggle = sessionHolder.GetComponentInChildren<Toggle>();
            
            if (!toggle.isOn)
            {
                selectedSessionId = sessionHolder.id;
                //Debug.Log("sectim bak id falan setledim  : " + _sessionId);
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
        
        public void JoinSession()
        {
            BoltMatchmaking.JoinSession(_sessionList[selectedSessionId].Session);
        }
    
        public void OpenCreateRoomPanel()
        {
            createRoomPanel.SetActive(true);
        }

        #endregion
    } 

}

