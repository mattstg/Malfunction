﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour {

    public Text mainText;
    string[] jsonHeaders = { "Tutorial_1", "Tutorial_2", "Tutorial_3" };
    int headerIndex = 0;

    public void Initialize()
    {
        mainText.text = LangDict.Instance.GetText(jsonHeaders[0]);
    }

    public void UpdateTutorialPanel()
    {

    }

    public void NextButtonPressed()
    {
        headerIndex++;
        if (headerIndex >= 3)
            EndTutorial();
        else
            mainText.text = LangDict.Instance.GetText(jsonHeaders[headerIndex]);
    }

    private void EndTutorial()
    {
        gameObject.SetActive(false);
        GameFlow.instance.TutorialFinished();
    }
}