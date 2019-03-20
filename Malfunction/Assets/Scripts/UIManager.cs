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
    public void Initialize(LoLFunction firstLolFunction)
    {
        uiLinks = GameFlow.uiLinks;
        ChangeLolFunction(firstLolFunction);
    }

    public void Update()
    {

    }

    public void ChangeLolFunction(LoLFunction newLolFunction)
    {
        uiLinks.questionText.text = newLolFunction.ToString();
        uiLinks.sampleInputText.text = GV.OutputSampleInput(GV.CleanseForDifficultyLevel(newLolFunction.inputVars, newLolFunction.difficultyLevel),newLolFunction.difficultyLevel);    
    }

    public void ChangeScienceAmt(int newAmt)
    {
        uiLinks.scienceAmt.text = newAmt.ToString();
    }
}
