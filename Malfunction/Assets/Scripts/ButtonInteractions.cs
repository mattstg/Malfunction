using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractions : MonoBehaviour {
    //This script is for events for buttons in scene, to communicate with the backend
    GameManager gameManager;

    public void Awake()
    {
        
    }

    public void WeaponSelected(int i)
    {
        GameFlow.instance.BuyBuilding((GameManager.BuyableBuilding)i);
    }

    public void SubmitAnswerPressed()
    {
        int ans = int.Parse(GameFlow.uiLinks.ansInputField.text);
        GameFlow.instance.SubmitButtonPressed(ans);
    }

    public void GameOverButtonPressed()
    {
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.MainMenu);
    }
}
