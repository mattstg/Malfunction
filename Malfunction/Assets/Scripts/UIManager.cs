using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager  {

    #region Singleton
    private static UIManager instance;

    private UIManager() { }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
    #endregion

    UIGameLinks uiLinks;
    bool answerFocused = false;

    public void Initialize(LoLFunction firstLolFunction)
    {
        uiLinks = GameFlow.uiLinks;
        ChangeLolFunction(firstLolFunction);
        SetPlaceholderContainerActive(!answerFocused);
    }

    public void Update()
    {
        if (uiLinks.ansInputField.isFocused != answerFocused)
        {
            answerFocused = uiLinks.ansInputField.isFocused;
            SetPlaceholderContainerActive(!answerFocused);
        }
        if (Input.GetKey(KeyCode.Return) && uiLinks.ansInputField.text != "")
            uiLinks.buttonInteractions.SubmitAnswerPressed();
    }

    public void ChangeLolFunction(LoLFunction newLolFunction)
    {
        uiLinks.questionText.text = newLolFunction.ToString();
        uiLinks.sampleInputText.text = GV.OutputSampleInput(newLolFunction.inputVars);
    }

    public void ChangeScienceAmt(int newAmt)
    {
        uiLinks.scienceAmt.text = newAmt.ToString();
    }

    public void SetPlaceholderContainerActive(bool active)
    {
        uiLinks.ansPlaceholderContainer.SetActive(active);
    }
}
