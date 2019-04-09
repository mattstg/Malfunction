using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Stage { Uninitialized = 0, Initialized = 1, GameRunning = 2, GameOver = 3 }
    public enum BuyableBuilding { Sam = 0, Nuke = 1, Shield = 2 }
    private Stage stage = Stage.Uninitialized;
    public Queue<float> waveTimes = new Queue<float>(new[] { 24.8f, 51.6f, 82.2f, 127.5f, 181.1f, 246.1f, 280.3f, 350f, 400.7f, 430f,450f,470f,500f});
    public int currentWave = 0;

    public SceneObjectManager objManager;
    public BuffManager bman;

    public float gameTime = 0;
    public float gameTimeModifier = 5f;
    
    public bool buildTrigger = false;

    public void UpdateGameManager()
    {
        Refresh(Time.deltaTime * gameTimeModifier);
    }

    public void Initialize()
    {
        objManager.Initialize(this);
        stage = Stage.Initialized;
        bman = new BuffManager(this);
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
                CheckNewWave();
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
        return objManager.spawnManager.city.BuildNewBuilding(BuildBuilding(typeToBuy));

        /*
        if(objManager.spawnManager.city.availableSlots > 0)
        {
            Debug.Log(objManager.spawnManager.city.availableSlots);
            SpawnManager.CitySlot slot = objManager.spawnManager.city.PopSlot();

            if(slot.slotID != -1)
            {
                ((BO_Static)objManager.SpawnObjectFromPool(BuildBuilding(typeToBuy), slot.position)).AssignCitySlot(slot);
                return true;
            }
            else
            {
                Debug.LogError("Pop Returned an empty city slot... full? Count: " + objManager.spawnManager.city.availableSlots);
                return false;
            }
            
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
                    //toReplace.gameObject.SetActive(false);
                    ((BO_Static)objManager.SpawnObjectFromPool(BuildBuilding(typeToBuy), slot.position)).AssignCitySlot(slot);
                    return true;
                }
            }
        } */
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

    public void CheckNewWave()
    {
        if (waveTimes.Count > 0 && gameTime + 2f >= waveTimes.Peek())
        {
            waveTimes.Dequeue();
            currentWave++;
            UIManager.Instance.NextWave(currentWave);
        }
    }
}
