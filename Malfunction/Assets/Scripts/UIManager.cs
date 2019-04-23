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
    float graphAspect;

    private float timer = 0f;
    private bool incomingWave = false;
    private readonly float totalTime = 5f;
    private readonly float fadeInTime = 4.75f;
    private readonly float fadeOutTime = 2f;

    public void Initialize(LoLFunction firstLolFunction)
    {
        uiLinks = GameFlow.uiLinks;
        SetPlaceholderContainerActive(!answerFocused);
        Rect rect = uiLinks.graph.rect;
        graphAspect = (rect.width > 0) ? rect.height / rect.width : 0f;
        timer = 0f;
        SetIncomingWaveAlpha(0f);
        ChangeLolFunction(firstLolFunction);
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
        if (incomingWave)
            UpdateIncomingWave();
    }

    public void UpdateIncomingWave()
    {
        timer -= Time.deltaTime;
        if (timer > fadeInTime)
        {
            SetIncomingWaveAlpha((timer - totalTime) / (fadeInTime - totalTime));
        }
        else if (timer < fadeOutTime)
        {
            if (timer <= 0f)
            {
                timer = 0f;
                incomingWave = false;
            }
            SetIncomingWaveAlpha(timer / fadeOutTime);
        }
        else
            SetIncomingWaveAlpha(1f);
    }

    public void SetIncomingWaveAlpha(float alpha)
    {
        Color c = uiLinks.incomingWavePanel.color;
        c.a = Mathf.Clamp01(alpha);
        uiLinks.incomingWavePanel.color = uiLinks.incomingWaveText.color = c;
    }

    public void ChangeLolFunction(LoLFunction newLolFunction)
    {
        if (!newLolFunction.isDumbGraphFunction)
        {
            uiLinks.questionText.text = newLolFunction.ToString();
            uiLinks.sampleInputText.text = GV.OutputSampleInput(newLolFunction.inputVars);
            uiLinks.graphFunctionObject.SetActive(false);
            uiLinks.questionText.gameObject.SetActive(true);
        }
        else
        {
            //uiLinks.questionText.text = newLolFunction.ToString();
            uiLinks.sampleInputText.text = GV.OutputSampleInput(newLolFunction.inputVars);
            uiLinks.graphFunctionObject.SetActive(true);
            uiLinks.questionText.gameObject.SetActive(false);

            float slope = newLolFunction.coefficents[0];
            if (newLolFunction.isGraphCoefficientInverse)
                slope = 1f / slope;
            int offset = newLolFunction.coefficents[3];
            //int output = newLolFunction.Solve();

            int scale = 1;
            //while (output > (scale * 8))
            //    scale *= 2;

            for (int i = 0; i < 5; i++)
                uiLinks.graphTextX[i].text = uiLinks.graphTextY[i].text = (i * scale * 2).ToString();

            float curveOffset = 0.1111f * offset / scale;
            uiLinks.graphCurve.anchorMin = new Vector2(uiLinks.graphCurve.anchorMin.x, curveOffset);
            uiLinks.graphCurve.anchorMax = new Vector2(uiLinks.graphCurve.anchorMax.x, curveOffset);

            float curveAngle = Mathf.Atan(slope * graphAspect) * 180f / Mathf.PI;
            uiLinks.graphCurve.localEulerAngles = new Vector3(uiLinks.graphCurve.localEulerAngles.x, uiLinks.graphCurve.localEulerAngles.y, curveAngle);
        }
    }

    public void ChangeScienceAmt(int newAmt)
    {
        uiLinks.scienceAmt.text = newAmt.ToString();
    }

    public void SetPlaceholderContainerActive(bool active)
    {
        uiLinks.ansPlaceholderContainer.SetActive(active);
    }

    public void NextWave(int wave)
    {
        uiLinks.waveText.text = wave.ToString();
        SetIncomingWaveAlpha(0f);
        timer = totalTime;
        incomingWave = true;
    }
}
