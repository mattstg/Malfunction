using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitor : MonoBehaviour {

	public void ChangeScene(string toScene)
    {
        CurrentState cs = (CurrentState)System.Enum.Parse(typeof(CurrentState), toScene);
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(cs);
    }
}
