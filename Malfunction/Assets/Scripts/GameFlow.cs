using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameFlow : Flow
{
    ChildBot childBot;

    //LevelPkg currentLevelPkg;
    public static GameFlow instance;
    public static UIGameLinks uiLinks;
    public static GameManager gameManager;
    LoLFunction currentLevelFunction;
    
    int numberOfStacksSolved;
    int numberOfStacksNotSolved;
    float time = 0f;
    public int amtOfScience = 0;
    int[] buildingPrices = new int[3]{ 3, 4, 4 };
    
    float winningStreak;
    bool isTutorial = false;

    public override void Initialize(int progressNumber)
    {
        instance = this; //half sad singleton
        uiLinks = GameObject.FindObjectOfType<UIGameLinks>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.Initialize();

        currentLevelFunction = QuestionBank.Instance.Initialize();
        UIManager.Instance.Initialize(currentLevelFunction);
      
        numberOfStacksSolved = 0;
        numberOfStacksNotSolved = 0;
        UIManager.Instance.ChangeScienceAmt(amtOfScience);
        winningStreak = 0;
        base.Initialize(progressNumber);

        if (ChildBot.childBotActive)
            childBot = new ChildBot();
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
    }

    public override void Update(float dt)
    {
        if (initialized && !isTutorial)
        {
            time += dt;
            UIManager.Instance.Update();
            gameManager.UpdateGameManager();
            UpdateColorLerp();
            if (ChildBot.childBotActive)
                UpdateChildBot();
        }
    }

    private void UpdateChildBot()
    {
        if(childBot.ShouldChildAttempt(numberOfStacksSolved))
        {
            if (childBot.AttemptSolve(numberOfStacksSolved))
                SolvedLevelPackage();
            else
                IncorrectLevelPackageGuess();
        }
    }

    public void SubmitButtonPressed(int attempt)
    {
        if (isTutorial)
            return;

        bool correctFunc = currentLevelFunction.Solve() == attempt;

        //winningStreak = (correctFunc) ? winningStreak + 1 : 0;
        //gameManager.SetStreak(winningStreak);

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
            bool buildSuccessful = gameManager.BuyBuilding(toBuy); //also is the line that builds
            if (buildSuccessful)
            {
                amtOfScience -= buildingPrices[i];
                UIManager.Instance.ChangeScienceAmt(amtOfScience);
            } //else thing wasnt built
        }
    }

    

    private void SolvedLevelPackage()
    {
        numberOfStacksSolved++;
        int pts = Mathf.Max(currentLevelFunction.currentLevel / 15,1);
        currentLevelFunction = QuestionBank.Instance.Pop();
        //LoadLevelPkg(LevelPkg.GenerateLevelPackage(currentDifficulty), currentDifficulty);
        ProgressTracker.Instance.SetScore(numberOfStacksSolved);
        ProgressTracker.Instance.SubmitProgress(2);        
        //LOLAudio.Instance.PlayAudio("PositiveFeedback");
        UIManager.Instance.ChangeLolFunction(currentLevelFunction);
        amtOfScience += pts;
        UIManager.Instance.ChangeScienceAmt(amtOfScience);

        winningStreak++;
        //gameManager.SetStreak(winningStreak);

        uiLinks.scienceAmt.color = Color.green;
        lerpTimeRemaining = colorLerpTotalTime;
        lerping = true;
        fromColor = Color.green;
    }

    private void IncorrectLevelPackageGuess()
    {
        numberOfStacksNotSolved++;
        //LOLAudio.Instance.PlayAudio("NegativeFeedback");
        amtOfScience = Mathf.Clamp(amtOfScience - 1,0,int.MaxValue);
        UIManager.Instance.ChangeScienceAmt(amtOfScience);

        winningStreak = 0;
        //gameManager.SetStreak(winningStreak);

        uiLinks.scienceAmt.color = Color.red;
        lerpTimeRemaining = colorLerpTotalTime;
        lerping = true;
        fromColor = Color.red;
    }

    bool lerping;
    float lerpTimeRemaining = 0;
    Color fromColor = Color.white;
    readonly float colorLerpTotalTime = 1.5f;    
    private void UpdateColorLerp()
    {
        if (lerping)
        {
            lerpTimeRemaining -= Time.deltaTime;
            float p = Mathf.Clamp01(lerpTimeRemaining / colorLerpTotalTime);
            uiLinks.scienceAmt.color = uiLinks.submitText.color = Color.Lerp(Color.white, fromColor, p);
            if (p <= 0)
            {
                lerping = false;
                uiLinks.scienceAmt.color = uiLinks.submitText.color = Color.white;
            }
        }
    }

    public void EndGame(bool win)
    {
        uiLinks.gameOverPanel.SetActive(true);
        string gameOverStats = GV.SecondsToTimeString(time);
        if (win)
        {
            uiLinks.gameOverStatsContainer.anchorMax = new Vector2(1f, 0.85f);
            uiLinks.victoryText.SetActive(true);
        }
        else
        {
            uiLinks.gameOverStatsContainer.anchorMax = new Vector2(1f, 1f);
            uiLinks.victoryText.SetActive(false);
        }
        float successRate = (numberOfStacksSolved + numberOfStacksNotSolved > 0) ? 100f * numberOfStacksSolved / (numberOfStacksSolved + numberOfStacksNotSolved) : 0;
        gameOverStats += "\r\n" + gameManager.currentWave;
        gameOverStats += "\r\n" + successRate.ToString("0.##") + "%";
        gameOverStats += "\r\n" + numberOfStacksSolved;
        gameOverStats += "\r\n" + numberOfStacksNotSolved;
        uiLinks.gameOverStats.text = gameOverStats;
        isTutorial = true; //acts like a pause
        ProgressTracker.Instance.SubmitProgress(3);        
    }
}
