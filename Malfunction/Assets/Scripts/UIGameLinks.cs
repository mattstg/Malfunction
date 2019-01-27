using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLinks : MonoBehaviour {

    public VerticalLayoutGroup historicalDataGroup;
    public VerticalLayoutGroup functionChoicesVerticalGroup;
    public Text timeRemainingText;
    public Text inputToSolveText;
    public InputField solutionInputField;
    public Button submitButton;



    public void ConfirmButton()
    {
        ((GameFlow)GV.ms.curFlow).ConfirmButtonPressed();    
    }
}
