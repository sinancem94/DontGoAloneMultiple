using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorerQuestionManager
{
    public enum QuestionType
    {
        OpenDoor,
        Custom
    }
    
    private MonoBehaviour _manager;
    private Button _choiceButton;
    
    public ExplorerQuestionManager(MonoBehaviour manager)
    {
        _manager = manager;
        _choiceButton = Resources.Load<Button>("Prefabs/UI/InGameUI");
    }

    public QuestionObject CreateYesNoQuestion(params Button.ButtonClickedEvent[] pressActions)
    {
        string question = "Open a random room ?"; 
        List<Button> choices = new List<Button>();
                
        Button choiceYes = GameObject.Instantiate(_choiceButton);
        choiceYes.GetComponentInChildren<Text>().text = "Yes";
        choiceYes.onClick = pressActions[0];
                
        Button choiceNo = GameObject.Instantiate(_choiceButton);
        choiceNo.GetComponentInChildren<Text>().text = "No";
        choiceYes.onClick = pressActions[1];
                
        choices.Add(choiceYes);
        choices.Add(choiceNo);
        QuestionObject questionObject = new QuestionObject(question,choices);
                
        return questionObject;
    }

    public QuestionObject CreateCustomQuestion(params Button.ButtonClickedEvent[] pressActions)
    {
        string question = "Open a random room ?"; 
        List<Button> choices = new List<Button>();
                
        Button choiceYes = GameObject.Instantiate(_choiceButton);
        choiceYes.GetComponentInChildren<Text>().text = "Yes";
        choiceYes.onClick = pressActions[0];
                
        Button choiceNo = GameObject.Instantiate(_choiceButton);
        choiceNo.GetComponentInChildren<Text>().text = "No";
        choiceYes.onClick = pressActions[1];
                
        choices.Add(choiceYes);
        choices.Add(choiceNo);
        QuestionObject questionObject = new QuestionObject(question,choices);
                
        return questionObject;
    }


    /*  public IEnumerator WaitForAnswer()
      {
              
          //while()
      }*/
}
