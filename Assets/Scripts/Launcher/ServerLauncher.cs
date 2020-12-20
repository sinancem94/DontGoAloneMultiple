using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Bolt.Matchmaking;
using Bolt.Photon;
using TMPro;
using UdpKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Launcher
{
    public class ServerLauncher : MonoBehaviour
    {
        private GameObject _scenesGroup;
        private string _selectedScene;
        private GameObject _sceneListElement;
        
        public string roomId = "";
        private int _size = -1;
        
        void OnEnable()
        {
            //this was a client instance until now sor shutdown client and start server when it`s ended
            BoltLauncher.Shutdown();
            
            _scenesGroup = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            _sceneListElement = Resources.Load<GameObject>("Prefabs/UI/SceneListElement");
            
            UpdateScenesList();
        }

        private void OnDisable()
        {
            roomId = "";
            _size = -1;
        }

        void UpdateScenesList()
        {
            foreach (string value in BoltScenes.AllScenes)
            {
                if (SceneManager.GetActiveScene().name != value)
                {
                    GameObject newListElement = Instantiate(_sceneListElement, _scenesGroup.transform);
                    newListElement.GetComponentInChildren<TextMeshProUGUI>().text = value;
                }
            }

        }

        public void CreateRoom()
        {
            // Create some room custom properties
            PhotonRoomProperties roomProperties = new PhotonRoomProperties();

            string gameType = "non";
            roomProperties.AddRoomProperty("t", gameType); // ex: game type
            roomProperties.AddRoomProperty("m", "BuildingBlocks"); // ex: map id

            roomProperties.IsOpen = true;
            roomProperties.IsVisible = true;

            // If RoomID was not set, create a random one
            if (roomId.Length == 0)
            { 
                roomId = Guid.NewGuid().ToString();
            }

            // Create the Photon Room
            BoltMatchmaking.CreateSession(
                sessionID: roomId,
                token: roomProperties,
                sceneToLoad: "BuildingBlocks"
            );
        }
        
        
        #region UIButtonCalls

        public void OnEndEnterName(TMP_InputField inputField)
        {
            Debug.Log($"player name is {inputField.text}");
        }

        public void OnEndEnterSize(TMP_InputField inputField)
        {
            try
            {
                int size = int.Parse(inputField.text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);

                if (size < 1)
                    size = 1;
                else if (size > 4)
                    size = 4;

                _size = size;

                inputField.text = _size.ToString();

                Debug.Log("Entered size " + _size);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public void OnSelectScene(Transform t)
        {
            Debug.Log("Pressed Toggle");
            Toggle toggle = t.GetComponentInChildren<Toggle>();
            
            if (!toggle.isOn)
            {
                _selectedScene = t.GetComponentInChildren<TextMeshProUGUI>().text;
                Debug.Log($"Selected scene {_selectedScene} {_selectedScene.Length}");
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }

        public void OnCreateRoom()
        {
            Debug.LogWarning("Trying to start server");
            /*if (_selectedScene.Length <= 0)
            {
                Debug.LogError("No scene selected");
                return;
            }*/

            if (_size <= 0)
            {
                Debug.LogError($"Invalid Size {_size}");
                return;
            }
            
            BoltConfig config = BoltRuntimeSettings.instance.GetConfigCopy();

            config.serverConnectionLimit = _size;
            BoltLauncher.StartServer(config);
            
            Debug.Log("Started starting server");
        }

        public void OnExitServerLauncher()
        {
            //shutdown if bolt is running as server
            if(BoltNetwork.IsRunning && BoltNetwork.IsServer)
                BoltLauncher.Shutdown();
            else
                BoltLauncher.StartClient();
            
            this.gameObject.SetActive(false);
        }

        #endregion


    }

}

