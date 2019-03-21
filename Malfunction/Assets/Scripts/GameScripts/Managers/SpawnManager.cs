using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public SceneObjectManager manager;

    public Transform planetFloor;
    public Transform sky;

    public float buildingSeperation = 0.5f;
    public int citySize = 10;
    public int numberOfSamTurrets = 10;
    
    public float startTimePerAsteroid = 2;
    public float maxTimePerAsteroid = 0.25f;
    public float gameTimeToMaxTimePerAsteroid = 120;

    public float TimePerAsteroid => Mathf.Lerp(startTimePerAsteroid, maxTimePerAsteroid, manager.manager.gameTime/gameTimeToMaxTimePerAsteroid);

    private float asteroidClock = 0;

    public int samsToSpawn =0;
    public int nukeLaunchersToSpawn = 0;
    public int shieldGeneratorsToSpawn = 15;

    public CityManager city;

    public static Vector3 Shift(Vector3 origion, float xOffset)
    {
        return new Vector3(origion.x + xOffset, origion.y, 0);
    }

    public void Initialize()
    {
        city = new CityManager(this);
        
    }


    public void Refresh(float dt)
    {

        if (asteroidClock > TimePerAsteroid)
        {
            asteroidClock = 0;
            float random = Random.Range(-citySize, citySize);
            Vector3 spawnPos = new Vector3(sky.position.x + random, sky.position.y, 0);
            manager.SpawnObjectFromPool(BaseObject.Type.Asteroid, spawnPos);
        }
        else
        {
            asteroidClock += dt;
        }
    }

    public void StartGame()
    {
        int samsToBuild = samsToSpawn;
        int nukesToBuild = nukeLaunchersToSpawn;
        int shieldsToSpawn = shieldGeneratorsToSpawn;
        while (city.availableSlots > 0)
        {
            CitySlot slot = city.PopSlot();
            if (samsToBuild > 0)
            {
                ((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.Sam, slot.position)).AssignCitySlot(slot);
                samsToBuild--;
            }
            else if (nukesToBuild > 0)
            {
                ((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.NukeLauncher, slot.position)).AssignCitySlot(slot);
                nukesToBuild--;
            }else if(shieldsToSpawn > 0)
            {
                ((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.ShieldGenerator, slot.position)).AssignCitySlot(slot);
                shieldsToSpawn--;
            }
            else
            {
                ((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.Building, slot.position)).AssignCitySlot(slot);
            }
        }
    }

    public void EndGame()
    {
        asteroidClock = 0;
    }

    public class CitySlot
    {
        public int slotID;
        public Vector3 position;

        public CitySlot(int id, Vector3 pos)
        {
            slotID = id;
            position = pos;
        }
    }

    public class CityManager
    {
        public int availableSlots { private set; get; }
        Vector3[] buildingSlots;
        bool[] slotOccupied;

        public CityManager(SpawnManager manager) //float citySize, float buildingSeperation)
        {
            float totalSize = manager.citySize * 2;
            int slots = Mathf.FloorToInt(totalSize / manager.buildingSeperation);
            buildingSlots = new Vector3[slots];
            slotOccupied = new bool[slots];
            availableSlots = slots;

            for (int i = 0; i < slots; i++)
            {
                buildingSlots[i] = Shift(manager.planetFloor.position, i * (manager.buildingSeperation) - (totalSize / 2));
            }
            
            Stack<Vector3> randomSlots = new Stack<Vector3>();
        }

        public CitySlot PopSlot()
        {
            if(availableSlots > 0)
            {
                availableSlots--;
                int random = Random.Range(0, buildingSlots.Length);
                while (slotOccupied[random])
                {
                    random = (random == buildingSlots.Length - 1) ? 0 : random + 1;
                }
                slotOccupied[random] = true;
                return new CitySlot(random, buildingSlots[random]);
            }
            else
            {
                Debug.LogError("Not Enough Slots...");
                return new CitySlot(-1, Vector2.zero);
            }
        }
        
        public void PushSlot(CitySlot citySlot)
        {
            availableSlots++;
            slotOccupied[citySlot.slotID] = false;
        }
    }

}
