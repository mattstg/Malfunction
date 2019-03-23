using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLinks : MonoBehaviour {

    public Text questionText;
    public Text scienceAmt;
    public Text streakText;
    public Text sampleInputText;
    public InputField ansInputField;
    public GameObject ansPlaceholderContainer;
    public Button submitButton;
    public GameObject gameOverPanel;
    public ButtonInteractions buttonInteractions;

    [Header("QuestionBalances")]
    public AnimationCurve inputvarUpperRange;
    public AnimationCurve inputvarLowerRange;
    public AnimationCurve coefficentUpperRange;
    public AnimationCurve coefficentLowerRange;
    public AnimationCurve numberOfCoefficents;
    public AnimationCurve chanceOfConstant;

}
