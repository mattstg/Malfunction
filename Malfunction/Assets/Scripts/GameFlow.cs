using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameFlow : Flow
{
    GameObject historicalDataTabPrefab;
    GameObject funcButtonPrefab;

    LevelPkg currentLevelPkg;
    UIGameLinks uiLinks;
    LoLFunction selectedFuncAsAnswer;

    int currentDifficulty;
    float timeRemaining = 120;

    const float timeAddedForCorrect = 10;
    const float timeRemovedForWrong = 5;


    public override void Initialize(int progressNumber)
    {
        historicalDataTabPrefab = Resources.Load<GameObject>("UI/Prefabs/HistoricalDataTab");
        funcButtonPrefab = Resources.Load<GameObject>("UI/Prefabs/FuncButton");
        uiLinks = GameObject.FindObjectOfType<UIGameLinks>();
        initialized = true;
        currentDifficulty = 0;
        LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
    }

    public override void Update(float dt)
    {
        if (initialized)
        {
            timeRemaining -= dt;
            uiLinks.timeRemainingText.text = timeRemaining.ToString();
            if(timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    private void LoadLevelPkg(LevelPkg _toLoad, int difficultyLevel)
    {
        currentLevelPkg = _toLoad;
        foreach (Transform t in uiLinks.historicalDataGroup.transform)
            GameObject.Destroy(t.gameObject);
        foreach(Vector3Int sampleInput in currentLevelPkg.sampleInputs) //Load all historical data
        {
            GameObject dataTab = GameObject.Instantiate(historicalDataTabPrefab);
            int output = currentLevelPkg.lolFunc.Solve(sampleInput);
            dataTab.GetComponentInChildren<Text>().text = GV.OutputSampleInput(sampleInput, difficultyLevel) + " => " + output;
            dataTab.transform.SetParent(uiLinks.historicalDataGroup.transform);
        }
        //Load all function options
        List<LoLFunction> lolfuncs = new List<LoLFunction>();
        lolfuncs.AddRange(currentLevelPkg.functionOptions);
        lolfuncs.Add(currentLevelPkg.lolFunc);
        lolfuncs = lolfuncs.OrderBy(x => Random.value).ToList();
        foreach (Transform t in uiLinks.functionChoicesVerticalGroup.transform)
            GameObject.Destroy(t.gameObject);
        foreach (LoLFunction lolFunc in lolfuncs) //Load all historical data
        {
            GameObject dataTab = GameObject.Instantiate(funcButtonPrefab);
            dataTab.GetComponentInChildren<Text>().text = lolFunc.ToString(difficultyLevel);
            dataTab.transform.SetParent(uiLinks.functionChoicesVerticalGroup.transform);
            LoLFunction f = lolFunc; //Hard to explain why this might be nescarry
            dataTab.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(() => { SelectFuncAsAnswer(f); }));
        }

        uiLinks.inputToSolveText.text = currentLevelPkg.inputToSolve.ToString();
    }

    public void SelectFuncAsAnswer(LoLFunction lf)
    {
        selectedFuncAsAnswer = lf;
    }

    public void ConfirmButtonPressed()
    {
        int solvedOutputGuess = int.Parse(uiLinks.solutionInputField.text);
        bool correctFunc = (currentLevelPkg.lolFunc == selectedFuncAsAnswer);
        if (solvedOutputGuess == currentLevelPkg.outputToSolve && correctFunc)
            SolvedLevelPackage();
        else
            IncorrectLevelPackageGuess();
    }

    private void SolvedLevelPackage()
    {
        currentDifficulty++;
        LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
        timeRemaining += timeAddedForCorrect;
        ProgressTracker.Instance.SetScore(currentDifficulty);
        ProgressTracker.Instance.SubmitProgress(7);
        Debug.Log("LEVEL PASSED");
        LOLAudio.Instance.PlayAudio("PositiveFeedback");
    }

    private void IncorrectLevelPackageGuess()
    {
        timeRemaining -= timeRemovedForWrong;
        Debug.Log("Incorrect guess you fucking failure of a child");
        LOLAudio.Instance.PlayAudio("NegativeFeedback");
    }

    private void EndGame()
    {
        ProgressTracker.Instance.SubmitProgress(8);
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.PostGame);
    }
}
