using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonInteractions : MonoBehaviour {

    public void TutorialNextPresed()
    {
        TutorialFlow.tutorialFlow.TutorialNextPresed();
    }

    public void InputSubmitPressed()
    {
        TutorialFlow.tutorialFlow.InputSubmitPressed();
    }
}
