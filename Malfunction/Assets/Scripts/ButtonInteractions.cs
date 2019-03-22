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
        int ans = int.Parse(GameFlow.uiLinks.ansInputField.text);
        GameFlow.instance.SubmitButtonPressed(ans);
        uiGameLinks.ansInputField.text = "";
        uiGameLinks.ansInputField.Select();
        uiGameLinks.ansInputField.ActivateInputField();
    }

    public void GameOverButtonPressed()
    {
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game);
    }

    public void MusicSelected(int i)
    {
        LOLAudio.Instance.PlayBackgroundAudio("bgMusic" + i);
    }
}
