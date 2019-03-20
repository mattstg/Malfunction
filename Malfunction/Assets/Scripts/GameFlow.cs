using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameFlow : Flow
{
    //LevelPkg currentLevelPkg;
    public static GameFlow instance;
    public static UIGameLinks uiLinks;
    LoLFunction currentLevelFunction;
    
    int numberOfStacksSolved;
    public int amtOfScience = 5;

    const float timeAddedForCorrect = 10;
    const float timeRemovedForWrong = 5;


    public override void Initialize(int progressNumber)
    {
        instance = this; //half sad singleton
        uiLinks = GameObject.FindObjectOfType<UIGameLinks>();
        
        currentLevelFunction = QuestionBank.Instance.Initialize();
        UIManager.Instance.Initialize(currentLevelFunction);
        initialized = true;
        numberOfStacksSolved = 0;
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
    }

    public override void Update(float dt)
    {
        if (initialized)
        {
            UIManager.Instance.Update();
        }
    }
    
    public void SubmitButtonPressed(int attempt)
    {
        bool correctFunc = currentLevelFunction.Solve() == attempt;
        if (correctFunc)
            SolvedLevelPackage();
        else
            IncorrectLevelPackageGuess();
    }

    private void SolvedLevelPackage()
    {
        numberOfStacksSolved++;
        currentLevelFunction = QuestionBank.Instance.Pop();
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
        ProgressTracker.Instance.SetScore(numberOfStacksSolved);
        ProgressTracker.Instance.SubmitProgress(7);
        LOLAudio.Instance.PlayAudio("PositiveFeedback");
        UIManager.Instance.ChangeLolFunction(currentLevelFunction);
}

    private void IncorrectLevelPackageGuess()
    {
        Debug.Log("Incorrect guess you fucking failure of a child");
        LOLAudio.Instance.PlayAudio("NegativeFeedback");
        amtOfScience--;
        UIManager.Instance.ChangeScienceAmt(amtOfScience);
    }

    private void EndGame()
    {
        ProgressTracker.Instance.SubmitProgress(8);
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.PostGame);
    }
}
