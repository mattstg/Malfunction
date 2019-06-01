using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractions : MonoBehaviour {
    //This script is for events for buttons in scene, to communicate with the backend
    public UIGameLinks uiGameLinks;
    GameManager gameManager;

    public void WeaponSelected(int i)
    {
        GameFlow.instance.BuyBuilding((GameManager.BuyableBuilding)i);
    }

    public void SubmitAnswerPressed()
    {
        if (GameFlow.uiLinks.ansInputField.text != "")
        {
            int ans = int.Parse(GameFlow.uiLinks.ansInputField.text);
            GameFlow.instance.SubmitButtonPressed(ans);
            uiGameLinks.ansInputField.text = "";
            uiGameLinks.ansInputField.Select();
            uiGameLinks.ansInputField.ActivateInputField();
        }
    }

	public static bool firstPlay = true;
    public void GameOverButtonPressed()
    {
		ProgressTracker.Instance.SubmitProgress(8);
		GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game);
		firstPlay = false;
    }

    public void QuitLoLGamePressed()
    {
		ProgressTracker.Instance.SubmitProgress(8);
		LoLSDK.LOLSDK.Instance.CompleteGame();
    }
    
}
