using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour {

    public GameObject audioButtonParents;
    public Text mainText;
    string[] jsonHeaders = { "Tutorial_1", "Tutorial_2", "Tutorial_3","Tutorial_4" };
    int headerIndex = 0;

    public void Initialize()
    {
        audioButtonParents.SetActive(false);
        mainText.text = LangDict.Instance.GetText(jsonHeaders[0]);
    }

    public void UpdateTutorialPanel()
    {

    }

    public void NextButtonPressed()
    {
        headerIndex++;
        if(headerIndex == 3)
        {
            audioButtonParents.SetActive(true);
        }
        else if (headerIndex >= 4)
        {
            audioButtonParents.SetActive(false);
            EndTutorial();
        }
        else
            mainText.text = LangDict.Instance.GetText(jsonHeaders[headerIndex]);
    }

    private void EndTutorial()
    {
        gameObject.SetActive(false);
        GameFlow.instance.TutorialFinished();
    }
}
