using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace ExplorerClient.ExplorerUI
{
    public class ExplorerUi : MonoBehaviour
    {
        public Transform InGamePanel;
        public Transform QuestionPopUp;

        private Text _mainMessage;
        private Text _logMessages;
        private StatPanel _statsPanel;

        private GridLayout _questionChoices;
        private Text _questionBody;
        
        public void Start()
        {
            _statsPanel = GetComponentInChildren<StatPanel>();

            _mainMessage = InGamePanel.GetComponentInChildren<Text>();
            _logMessages = InGamePanel.GetChild(1).GetComponent<Text>();


            _questionChoices = QuestionPopUp.GetComponentInChildren<GridLayout>();
            _questionBody = QuestionPopUp.GetComponentInChildren<Text>();
        }

        public void SetMainMessage(string message)
        {
            _mainMessage.text = message;
        }
    
        public void SetStatUi(ExplorerSlate_SO slate)
        {
            _statsPanel.SetCharacterName(slate.CharachterName);
            _statsPanel.SetStats(slate);
        }

        public void OpenQuestionPopUp(QuestionObject question)
        {
            _questionBody.text = question.Question;
            foreach (var choice in question.Choices)
            {
                choice.transform.parent = _questionChoices.transform;
            }
        }
    }
}
