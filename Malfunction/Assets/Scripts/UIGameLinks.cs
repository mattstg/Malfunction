using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLinks : MonoBehaviour {

    public Text questionText;
    public Text scienceAmt;
    public Text waveText;
    public Text sampleInputText;
    public InputField ansInputField;
    public GameObject ansPlaceholderContainer;
    public Button submitButton;
    public Text submitText;
    public GameObject gameOverPanel;
    public Text gameOverStats;
    public ButtonInteractions buttonInteractions;
    public GameObject graphFunctionObject;
    public RectTransform graph;
    public List<Text> graphTextX;
    public List<Text> graphTextY;
    public RectTransform graphCurve;
    public Image incomingWavePanel;
    public Text incomingWaveText;


    [Header("QuestionBalances")]
    public AnimationCurve inputvarUpperRange;
    public AnimationCurve inputvarLowerRange;
    public AnimationCurve coefficentUpperRange;
    public AnimationCurve coefficentLowerRange;
    public AnimationCurve numberOfCoefficents;
    public AnimationCurve chanceOfConstant;
    public AnimationCurve timePerAstroid;

}
