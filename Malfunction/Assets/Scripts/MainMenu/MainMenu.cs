using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    bool freshGame;
    int progressPt;
    public Button mainButton;
    //public Button lessonAgainBtn;
    public Text mainTitleText;
    public Text loadingText;

    public void SDKLoaded(int _progressPt)
    {
        loadingText.gameObject.SetActive(false);
        progressPt = _progressPt;
        freshGame = (_progressPt <= 1);
        ProgressTracker.Instance.SubmitProgress(progressPt);
        mainButton.GetComponentInChildren<Text>().text = LangDict.Instance.GetText("StartButton") ;
        mainButton.gameObject.SetActive(true);
        //lessonAgainBtn.gameObject.SetActive(!freshGame);
        //lessonAgainBtn.GetComponentInChildren<Text>().text = LangDict.Instance.GetText("LessonButton");
        mainTitleText.text = LangDict.Instance.GetText("Title");
        mainTitleText.gameObject.SetActive(true);
        LOLAudio.Instance.PlayBackgroundAudio("bgMusic3");
    }

	public void StartPressed()
    {
        ProgressTracker.Instance.SubmitProgress(1); //Congrats the first arbitrary checkpoint is the start button
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Tutorial);
        //if (progressPt <= GV.LastTutorialProgressPoint)
        //{
        //    ProgressTracker.Instance.SubmitProgress(1);
        //    GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Tutorial);
        //}
        //else
        //    GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game);
    }
}
