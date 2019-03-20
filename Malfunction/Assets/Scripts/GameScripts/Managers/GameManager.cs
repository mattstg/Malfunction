using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Stage { Uninitialized = 0, Initialized = 1, GameRunning = 2, GameOver = 3 }
    private Stage stage = Stage.Uninitialized;

    public SceneObjectManager objManager;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Refresh(Time.deltaTime);
    }


    public void Initialize()
    {
        objManager.Initialize();
        stage = Stage.Initialized;
    }

    public void Refresh(float dt)
    {
        switch (stage)
        {
            case Stage.Uninitialized:
                break;
            case Stage.Initialized:
                GameStart();
                break;
            case Stage.GameRunning:
                objManager.Refresh(dt);
                break;
            case Stage.GameOver:
                break;
        }
    }

    public void GameStart()
    {
        stage = Stage.GameRunning;
    }

    public void GameEnd()
    {
        stage = Stage.GameOver;
    }


}
