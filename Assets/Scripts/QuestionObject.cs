using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionObject
{
    public string Question;
    public List<Button> Choices;

    public QuestionObject(string question,List<Button> choices)
    {
        Question = question;
        Choices = new List<Button>();
        Choices.AddRange(choices);
    }
}
