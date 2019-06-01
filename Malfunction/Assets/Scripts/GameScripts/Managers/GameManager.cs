using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Stage { Uninitialized = 0, Initialized = 1, GameRunning = 2, GameOver = 3 }
    public enum BuyableBuilding { Sam = 0, Nuke = 1, Shield = 2 }
    private Stage stage = Stage.Uninitialized;
	public Queue<float> waveTimes;// = new Queue<float>(new[] { 390f, 300f, 210f, 120f, 30f});   //{ 24.8f, 51.6f, 82.2f, 127.5f, 181.1f, 246.1f, 280.3f, 350f, 400.7f, 430f,450f,470f,500f});
    float[] timePerAsteroid = { .79f, .56f, .40f, .33f ,.26f, .185f };
    public int currentWave = 0;
    float timeBetweenWaves = 70;

    public SceneObjectManager objManager;
    public BuffManager bman;

    public float gameTime = 0;
    public float gameTimeModifier = 5f;
    public float timeRemaining = 480;
    public bool buildTrigger = false;

    public void Initialize()
    {
        waveTimes = new Queue<float>();
        float nextWave = 480 - timeBetweenWaves;
        while(nextWave >= 0)
        {
            waveTimes.Enqueue(nextWave);
            nextWave = nextWave - timeBetweenWaves; ;
        }


        objManager.Initialize(this);
        stage = Stage.Initialized;
        bman = new BuffManager(this);
    }

    public void UpdateGameManager()
    {
        Refresh(Time.deltaTime * gameTimeModifier);
    }

    private void StartNextWave(int waveNumber)
    {
        SpawnManager.instance.TimePerAsteroid = timePerAsteroid[currentWave - 1];// Mathf.Lerp(.85f, .2f, (currentWave / 6f));
        //Debug.Log("is now: " + SpawnManager.instance.TimePerAsteroid);
        StartCoroutine(StopWave());
    }

    IEnumerator StopWave()
    {
        yield return new WaitForSeconds(21);
        SpawnManager.instance.TimePerAsteroid = 1.9f - .12f * currentWave;
		if (currentWave >= 6)
		{
			SpawnManager.instance.TimePerAsteroid = 1.2f;
			yield return new WaitForSeconds(35);
			SpawnManager.instance.TimePerAsteroid = 100f;
		}
        //Debug.Log("back to normal: " + SpawnManager.instance.TimePerAsteroid);
    }

    private void UpdateTimeRemaining()
    {
        timeRemaining -= Time.deltaTime;
        int min = (int)(timeRemaining / 60);
        int sec = (int)(timeRemaining % 60);
        GameFlow.uiLinks.timeRemaining.text = min.ToString("0") + ":" + sec.ToString("00");
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
                UpdateTimeRemaining();
                objManager.Refresh(dt);
                CheckNewWave();
                if (CheckWinCondition())
                    GameEnd(true);
                else if (CheckEndCondition())
                    GameEnd(false);
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

    public void GameEnd(bool win)
    {
        stage = Stage.GameOver;
        objManager.EndGame();
        GameFlow.instance.EndGame(win);
    }

    private bool CheckEndCondition()
    {
        return objManager.activeBuildings.Count == 0;
    }

    private bool CheckWinCondition()
    {
        return timeRemaining <= 0f;
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
        if (waveTimes.Count > 0 && timeRemaining - 2f <= waveTimes.Peek())
        {
            waveTimes.Dequeue();
            currentWave++;
            UIManager.Instance.NextWave(currentWave);
            StartNextWave(currentWave);
        }
    }
}
