using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlow : Flow
{
    public delegate void tutEventDelg();
    public static TutorialFlow tutorialFlow;
    TutorialGameLinks tutGameLinks;

    Queue<tutEventDelg> delgStack;
    bool goNext;
    
    public override void Initialize(int progressNumber)
    {
        tutorialFlow = this;
        tutGameLinks = GameObject.FindObjectOfType<TutorialGameLinks>();
        //Initial setup
        //tutGameLinks


        delgStack = new Queue<tutEventDelg>();
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_1"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_2"); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.gridImg.gameObject.SetActive(true); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_3"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_4"); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.gridLinesRocket.gameObject.SetActive(true); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_5"); });

        //Solve the X,Y cordinate problem
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_6"); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.tutorialNextButton.SetActive(false); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.xFieldObject.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.yFieldObject.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.fieldSubmitButton.SetActive(true); });
        //Disable that stuff, go back to normal tutorial flow
        delgStack.Enqueue(() => { tutGameLinks.tutorialNextButton.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.fieldSubmitButton.SetActive(false); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.xFieldObject.SetActive(false); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.yFieldObject.SetActive(false); goNext = true; });        
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_7"); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.gridLinesAstroid.gameObject.SetActive(true); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_8"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_9"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_10"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_11"); });
        delgStack.Enqueue(() => { tutGameLinks.rocketFuncLine.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_12"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_13"); });
        //switching over to the micro text
        delgStack.Enqueue(() => { SetMicroPanelTutorialText("Tutorial_14"); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.tutorialText.transform.parent.gameObject.SetActive(false); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.microPanelText.transform.parent.gameObject.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.astroidFuncLine.SetActive(true); });        
        delgStack.Enqueue(() => { SetMicroPanelTutorialText("Tutorial_15"); });
        //Swap back to normal text
        delgStack.Enqueue(() => { tutGameLinks.tutorialText.transform.parent.gameObject.SetActive(true); goNext = true; });
        delgStack.Enqueue(() => { tutGameLinks.microPanelText.transform.parent.gameObject.SetActive(false); goNext = true; });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_16"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_17"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_18"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_19"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_20"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_21"); });
        delgStack.Enqueue(() => { SetTutorialText("Tutorial_22"); });
        //delgStack.Enqueue(() => { ProgressTracker.Instance.SubmitProgress(2->6); });
        delgStack.Enqueue(() => { GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game); });
        ProgressStack();

        //LOLAudio.Instance.ClearDisabledSounds();
        //initialized = true;
    }

    public override void Update(float dt)
    {
        //if (initialized)
        //{
        //             
        //}
        //ProgressTracker.Instance.SubmitProgress(2->6);  Submit all progress numbers, from 1 to 6 during tutorial, 7 and 8 are for game 

    }

    public void ProgressStack()
    {
        delgStack.Dequeue().Invoke();
        while (goNext)
        {
            goNext = false;
            delgStack.Dequeue().Invoke();
        }
    }

    public void SetTutorialText(string jsonName)
    {
        tutGameLinks.tutorialText.text = LangDict.Instance.GetText(jsonName);
    }

    public void SetMicroPanelTutorialText(string jsonName)
    {
        tutGameLinks.microPanelText.text = LangDict.Instance.GetText(jsonName);
    }

    public void TutorialNextPresed()
    {
        ProgressStack();
    }

    bool isFirstFieldSubmit = true;
    public void InputSubmitPressed()
    {
        if (isFirstFieldSubmit)
        {
            if (tutGameLinks.xFieldObject.GetComponentInChildren<UnityEngine.UI.InputField>().text == "5" && tutGameLinks.yFieldObject.GetComponentInChildren<UnityEngine.UI.InputField>().text == "10")
            {
                ProgressStack();
                isFirstFieldSubmit = false;
                
            }
            else
                tutGameLinks.tutorialText.text = LangDict.Instance.GetText("Tutorial_7_Wrong");
        }       
        else
        {
            if (tutGameLinks.outputField.text == "10")
            {
                ProgressStack();
                tutGameLinks.submitAnsButton.GetComponentInChildren<UnityEngine.UI.Text>().text = LangDict.Instance.GetText("Tutorial_NextButton");
                tutGameLinks.outputField.gameObject.SetActive(false);
            }
            else
                tutGameLinks.microPanelText.text = LangDict.Instance.GetText("Tutorial_14_Wrong");
        }     
    }



    private class TutorialEvent
    {
        public tutEventDelg tEvent;
        
        public TutorialEvent(tutEventDelg _tEvent)
        {
            tEvent = _tEvent;
        }
    }
}
