﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Stage { Uninitialized = 0, Initialized = 1, GameRunning = 2, GameOver = 3 }
    private Stage stage = Stage.Uninitialized;

    public SceneObjectManager objManager;

    public float gameTime = 0;

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
        objManager.Initialize(this);
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
                gameTime += dt;
                objManager.Refresh(dt);
                if (CheckEndCondition())
                    GameEnd();
                break;
            case Stage.GameOver:
                GameStart();
                break;
        }
    }

    public void GameStart()
    {
        stage = Stage.GameRunning;
        gameTime = 0;
        objManager.StartGame();
    }

    public void GameEnd()
    {
        stage = Stage.GameOver;
        objManager.EndGame();
    }

    private bool CheckEndCondition()
    {
        return objManager.activeBuildings.Count == 0;
    }
}