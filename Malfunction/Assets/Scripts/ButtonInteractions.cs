using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractions : MonoBehaviour {
    //This script is for events for buttons in scene, to communicate with the backend
	public void WeaponSelected(int i)
    {

    }

    public void SubmitAnswerPressed()
    {
        string ans = GameFlow.uiLinks.ansInputField.text;
    }
}
