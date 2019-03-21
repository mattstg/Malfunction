using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLinks : MonoBehaviour {

    public Text questionText;
    public Text scienceAmt;
    public Text sampleInputText;
    public InputField ansInputField;
    public Button submitButton;

    [Header("QuestionBalances")]
    public AnimationCurve inputvarUpperRange;
    public AnimationCurve inputvarLowerRange;
    public AnimationCurve coefficentUpperRange;
    public AnimationCurve coefficentLowerRange;
    public AnimationCurve numberOfCoefficents;
    public AnimationCurve chanceOfConstant;

}
