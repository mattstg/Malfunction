using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Stage { Uninitialized = 0, Initialized = 1, GameRunning = 2, GameOver = 3 }
    public enum BuyableBuilding { Sam = 0, Nuke = 1, Shield = 2 }
    private Stage stage = Stage.Uninitialized;

    public SceneObjectManager objManager;

    public float gameTime = 0;
    public float gameTimeModifier = .2f;
    
    public bool buildTrigger = false;

    public void UpdateGameManager()
    {
        Refresh(Time.deltaTime * gameTimeModifier);
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
                if (buildTrigger)
                {
                    buildTrigger = false;
                    BuyBuilding(BuyableBuilding.Sam);
                }
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
        GameFlow.instance.EndGame();
    }

    private bool CheckEndCondition()
    {
        return objManager.activeBuildings.Count == 0;
    }

    public bool BuyBuilding(BuyableBuilding typeToBuy)
    {
        if(objManager.spawnManager.city.availableSlots > 0)
        {
            Debug.Log(objManager.spawnManager.city.availableSlots);
            SpawnManager.CitySlot slot = objManager.spawnManager.city.PopSlot();
            ((BO_Static)objManager.SpawnObjectFromPool(BuildBuilding(typeToBuy), slot.position)).AssignCitySlot(slot);
            return true;
        }
        else
        {
            Building toReplace = objManager.GetRandomBuilding();
            if (toReplace == null)
                return false;
            else
            {
                if(toReplace.citySlot.slotID == -1)
                {
                    Debug.LogError("??!?");
                    return false;
                }
                else
                {
                    SpawnManager.CitySlot slot = new SpawnManager.CitySlot(toReplace.citySlot.slotID, toReplace.citySlot.position);
                    toReplace.ExternalDeath();
                    toReplace.gameObject.SetActive(false);
                    ((BO_Static)objManager.SpawnObjectFromPool(BuildBuilding(typeToBuy), slot.position)).AssignCitySlot(slot);
                    return true;
                }
            }
        }
    }

    public BaseObject.Type BuildBuilding(BuyableBuilding typeToBuy)
    {
        switch (typeToBuy)
        {
            case BuyableBuilding.Sam:
                return BaseObject.Type.Sam;
            case BuyableBuilding.Nuke:
                return BaseObject.Type.NukeLauncher;
            case BuyableBuilding.Shield:
                return BaseObject.Type.ShieldGenerator;
        }
        return BaseObject.Type.Building;
    }

    public void SetStreak(float currentStreak)
    {
        Debug.Log("Current Streak: " + currentStreak);
    }
}
