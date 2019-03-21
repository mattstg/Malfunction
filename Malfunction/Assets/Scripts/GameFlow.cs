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
    public static GameManager gameManager;
    public static TutorialPanel tutorialPanel;
    LoLFunction currentLevelFunction;
    
    int numberOfStacksSolved;
    public int amtOfScience = 5000;
    int[] buildingPrices = new int[3]{ 3, 5, 7 };

    const float timeAddedForCorrect = 10;
    const float timeRemovedForWrong = 5;
    bool isTutorial = true;
    int winningStreak = 0;

    public override void Initialize(int progressNumber)
    {
        instance = this; //half sad singleton
        uiLinks = GameObject.FindObjectOfType<UIGameLinks>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.Initialize();
        tutorialPanel = GameObject.FindObjectOfType<TutorialPanel>();
        tutorialPanel.Initialize();

        currentLevelFunction = QuestionBank.Instance.Initialize();
        UIManager.Instance.Initialize(currentLevelFunction);
        initialized = true;
        numberOfStacksSolved = 0;
        UIManager.Instance.ChangeScienceAmt(amtOfScience);
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
    }

    public override void Update(float dt)
    {
        if (initialized && !isTutorial)
        {
            UIManager.Instance.Update();
            gameManager.UpdateGameManager();
        }
        else if(isTutorial)
        {
            tutorialPanel.UpdateTutorialPanel();
        }
    }
    
    public void TutorialFinished()
    {
        isTutorial = false;
    }

    public void SubmitButtonPressed(int attempt)
    {
        if (isTutorial)
            return;

        bool correctFunc = currentLevelFunction.Solve() == attempt;

        winningStreak = (correctFunc) ? winningStreak + 1 : 0;
        gameManager.SetStreak(winningStreak);

        if (correctFunc)
            SolvedLevelPackage();
        else
            IncorrectLevelPackageGuess();

    }

    public void BuyBuilding(GameManager.BuyableBuilding toBuy)
    {
        if (isTutorial)
            return;

        int i = (int)toBuy;
        if(amtOfScience >= buildingPrices[i])
        {
            amtOfScience -= buildingPrices[i];
            gameManager.BuyBuilding(toBuy);
            UIManager.Instance.ChangeScienceAmt(amtOfScience);
        }
        else
        {
            Debug.Log("Not enough science to buy building");
        }
    }

    static bool submit7 = false;
    private void SolvedLevelPackage()
    {
        Debug.Log("Solved!");
        numberOfStacksSolved++;
        currentLevelFunction = QuestionBank.Instance.Pop();
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
        ProgressTracker.Instance.SetScore(numberOfStacksSolved);
        if (!submit7)
        {
            ProgressTracker.Instance.SubmitProgress(7);
            submit7 = true;
        }
        LOLAudio.Instance.PlayAudio("PositiveFeedback");
        UIManager.Instance.ChangeLolFunction(currentLevelFunction);
        UIManager.Instance.ChangeScienceAmt(++amtOfScience);
    }

    private void IncorrectLevelPackageGuess()
    {
        Debug.Log("Incorrect guess you fucking failure of a child");
        LOLAudio.Instance.PlayAudio("NegativeFeedback");
        amtOfScience--;
        UIManager.Instance.ChangeScienceAmt(amtOfScience);
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER");
        ProgressTracker.Instance.SubmitProgress(8);
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.PostGame);

    }
}
