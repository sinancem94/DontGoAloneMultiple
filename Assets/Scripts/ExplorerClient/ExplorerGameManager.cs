using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ExplorerClient
{
    [BoltGlobalBehaviour("BuildingBlocks")]
    public class ExplorerGameManager : BoltSingletonPrefab<ExplorerGameManager>
    {
        private ExplorerSlate_SO _slateSo;
        private ExplorerUI.ExplorerUi _explorerUi;
        private ExplorerMouseManager _explorerMouseManager;
        private ExplorerQuestionManager _explorerQuestionManager;
        
        private bool _managerStarted = false;
        
        public BetreyalBoard board;
        public ExplorerController explorer;
        public List<Transform> nameTags;

        public ExplorerSlate_SO GetCharacter => _slateSo;
        

        private void Update()
        {
            if (_managerStarted)
            {
                _explorerMouseManager.CastRay();
                UpdateExplorerCursor();
            }
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E");
            }
        
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Vector3 roomPos = board.GetRoom(explorer.state.GridPos).transform.position;
                ExplorerCamera.instance.BeamCamera(roomPos);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("B");
            }
        }

        private void OnDisable()
        {
            InputEventManager.inputEvent.onLeftClick -= Clicked;
        }

        void SetExplorerSlate()
        {
            _slateSo = board.currEquipments.GetExplorerSlate(explorer.state.Id);
            _explorerUi.SetStatUi(_slateSo);
        }

        void UpdateExplorerCursor()
        {
            Cursor.SetCursor(_explorerMouseManager.CurrentCursorTexture,Vector2.zero,CursorMode.ForceSoftware);
        }
        
        void ChangeAllNameTags()
        {
            
        }

        private void Clicked(Vector2 clickPos)
        {
            switch (_explorerMouseManager.CurrentCursor)
            {
                case ExplorerMouseManager.CursorLayer.Default:
                    explorer.MoveTo(_explorerMouseManager.ClickedWorldPosition);
                    break;
                case ExplorerMouseManager.CursorLayer.Action:
                case ExplorerMouseManager.CursorLayer.Interest:
                    break; 
                case ExplorerMouseManager.CursorLayer.Door: 
                    explorer.MoveTo(_explorerMouseManager.ClickedTransform.transform.position,true);
                    break;
                case ExplorerMouseManager.CursorLayer.Unreachable: 
                    break;
            }
        }

        private void PressedOpenNewDoor()
        {
            
        }

        private void PressedNo()
        {
            
        }
        
        #region public methods
        
        public void SetManager()
        {
            _explorerMouseManager = new ExplorerMouseManager(board.currEquipments);
            _explorerQuestionManager = new ExplorerQuestionManager(this);
            
            _explorerUi = FindObjectOfType<ExplorerUI.ExplorerUi>();
            if(_explorerUi == null)
                Debug.LogError("No UI script in canvas");
            
            //set events
            InputEventManager.inputEvent.onLeftClick += Clicked;
            
            SetExplorerSlate();
            _managerStarted = true;
        }

        public void AskQuestionToExplorer(ExplorerQuestionManager.QuestionType type)
        {
            switch (type)
            {
                case ExplorerQuestionManager.QuestionType.OpenDoor:
                    
                    Button.ButtonClickedEvent clickedYes = new Button.ButtonClickedEvent();
                    clickedYes.AddListener(PressedOpenNewDoor);
                    
                    Button.ButtonClickedEvent clickedNo = new Button.ButtonClickedEvent();
                    clickedNo.AddListener(PressedNo);
                    
                    QuestionObject questionObject = _explorerQuestionManager.CreateYesNoQuestion(clickedYes,clickedNo);
                    _explorerUi.OpenQuestionPopUp(questionObject);
                    break;
                default:
                    break;
            }
        }

        
        
        public void ExplorerAnsweredQuestion(ExplorerQuestionManager.QuestionType type)
        {
            
        }

        #endregion

    }
}
