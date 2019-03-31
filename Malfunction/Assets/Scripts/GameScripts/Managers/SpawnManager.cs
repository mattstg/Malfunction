using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour {
    public SceneObjectManager manager;

    public Transform planetFloor;
    public Transform sky;

    public float buildingSeperation = 0.5f;
    public int citySize = 10;
    
    public float startTimePerAsteroid = 2;
    public float maxTimePerAsteroid = 0.25f;
    public float gameTimeToMaxTimePerAsteroid = 120;

    public float TimePerAsteroid => GameFlow.uiLinks.timePerAstroid.Evaluate(manager.manager.gameTime); //Mathf.Lerp(startTimePerAsteroid, maxTimePerAsteroid, manager.manager.gameTime/gameTimeToMaxTimePerAsteroid);

    private float asteroidClock = 0;

    public int samsToSpawn =0;
    public int nukeLaunchersToSpawn = 0;
    public int shieldGeneratorsToSpawn = 15;

    public CityManager city;
    
    public void Initialize()
    {

        city = new CityManager(this);
    }


    public void Refresh(float dt)
    {
        Debug.Log("Game Time: " + manager.manager.gameTime);
        if (asteroidClock > TimePerAsteroid)
        {
            Debug.Log("Astroid summoned, time per astroid: " + TimePerAsteroid);
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
        while (CitySlot.availableSlots > 0)
        {
            CitySlot slot = CitySlot.GetRandomEmptySlot();
            if (samsToBuild > 0)
            {
                slot.SetOccupant((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.Sam, slot.position));
                samsToBuild--;
            }
            else if (nukesToBuild > 0)
            {
                slot.SetOccupant((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.NukeLauncher, slot.position));
                nukesToBuild--;
            }else if(shieldsToSpawn > 0)
            {
                slot.SetOccupant((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.ShieldGenerator, slot.position));
                shieldsToSpawn--;
            }
            else
            {
                slot.SetOccupant((BO_Static)manager.SpawnObjectFromPool(BaseObject.Type.Building, slot.position));
            }
        }
    }

    public void EndGame()
    {
        asteroidClock = 0;
    }

    public class CitySlot
    {
        private static Dictionary<BaseObject.Type, List<CitySlot>> boTypeDict;
        
        public static CitySlot nullSlot { private set; get; }
        private static CitySlot[] city;
        private static List<CitySlot> freeSlotList;
        public static int availableSlots = 0;

        private static List<CitySlot> TypeToList(BaseObject.Type t)
        {
            if (!boTypeDict.ContainsKey(t))
                boTypeDict.Add(t, new List<CitySlot>());
            return boTypeDict[t];
        }

        private static bool TypeAdd(CitySlot toAdd)
        {
            if (toAdd.hasOccupant)
            {
                TypeToList(toAdd.occupant.type).Add(toAdd);
                return true;
            }
            Debug.LogError("Type add on an empty city slot? fill city slot first...");
            return false;
        }

        private static bool TypeRemove(CitySlot toRemove)
        {
            if (toRemove.hasOccupant)
            {
                List<CitySlot> list = TypeToList(toRemove.occupant.type);
                if (list.Contains(toRemove))
                {
                    list.Remove(toRemove);
                    return true;
                }
                else
                {
                    Debug.Log("Trying to type remove an empty city slot.");
                    return false;
                }
            }
            Debug.LogError("Type add on an empty city slot? fill city slot first...");
            return false;
        }

        public static CitySlot[] CreateNewCity(SpawnManager manager)
        {
            boTypeDict = new Dictionary<BaseObject.Type, List<CitySlot>>();
            float totalSize = manager.citySize * 2;
            int nbSlots = Mathf.FloorToInt(totalSize / manager.buildingSeperation);
            city = new CitySlot[nbSlots];
            freeSlotList = new List<CitySlot>(nbSlots);
            availableSlots = nbSlots;

            for (int i = 0; i < nbSlots; i++)
            {
                city[i] = new CitySlot(i, new Vector3(manager.planetFloor.position.x + i * (manager.buildingSeperation) - (totalSize / 2), manager.planetFloor.position.y, 0));
                freeSlotList.Add(city[i]);
            }
            
            nullSlot = new CitySlot(-1, Vector3.zero);
            return city;
        }

        public static bool HasAvailableSlot => availableSlots > 0;

        public static CitySlot GetRandomEmptySlot()
        {
            if(availableSlots > 0)
            {
                if(freeSlotList.Count > 0)
                {
                    int rand = Random.Range(0, freeSlotList.Count);
                    return freeSlotList.ElementAt(rand);
                }
            }
            Debug.Log("Could not get empty slot, returning nullSlot...");
            return nullSlot;
        }

        public static CitySlot GetRandomSlotOfType(BaseObject.Type type)
        {
            if (boTypeDict.ContainsKey(type))
            {
                List<CitySlot> slotsOfType = boTypeDict[type];
                if (slotsOfType.Count == 0)
                    return nullSlot;
                int rand = Random.Range(0, slotsOfType.Count);
                return slotsOfType.ElementAt(rand);
            }
            Debug.Log("Could not get empty slot, returning nullSlot...");
            return nullSlot;
        }


        //Non Static
        public bool IsAvailable => !hasOccupant;
        
        public int slotID { private set; get; }
        public Vector3 position { private set; get; }
        public BO_Static occupant { private set; get; }
        private bool hasOccupant = false;
        public bool IsNull => slotID == -1;

        private CitySlot(int id, Vector3 pos)
        {
            slotID = id;
            position = pos;
            occupant = null;
            hasOccupant = false;
        }

        public bool SetOccupant(BO_Static newOcc)
        {
            if (IsAvailable)
            {
                if (freeSlotList.Remove(this))
                    availableSlots--;
                else
                    Debug.LogError("available slot desync?");
                hasOccupant = true;
                occupant = newOcc;
                if (!TypeAdd(this))
                {
                    Debug.LogError("type lookup dict desync?");
                }
                occupant.AssignCitySlot(this);
                return true;
            }
            return false;
        } 

        public bool EmptyOccupant()
        {
            if (!IsAvailable)
            {
                if (!freeSlotList.Contains(this))
                {
                    freeSlotList.Add(this);
                    availableSlots++;
                }
                else
                    Debug.LogError("available slot desync?");

                if (!TypeRemove(this))
                {
                    Debug.LogError("type lookup dict desync?");
                }
                hasOccupant = false;
                occupant = null;
                return true;
            }
            return false;
        }
    }

    public class CityManager
    {
        SpawnManager manager;
        public CityManager(SpawnManager newMan) //float citySize, float buildingSeperation)
        {
            manager = newMan;
            CitySlot.CreateNewCity(manager);
        }

        public bool BuildNewBuilding(BaseObject.Type typeToBuild)
        {
            if (CitySlot.HasAvailableSlot)
            {
                CitySlot s = CitySlot.GetRandomEmptySlot();
                if (!s.IsNull)
                {
                    s.SetOccupant(((BO_Static)manager.manager.SpawnObjectFromPool(typeToBuild, s.position)));
                    return true;
                }
                else
                {
                    Debug.LogError("Get Random Empty Slot error?");
                    return false;
                }
            }
            else
            {
                BaseObject.Type replaceType;
                CitySlot s = CitySlot.nullSlot;
                switch (typeToBuild)
                {
                    case BaseObject.Type.Sam:
                        replaceType = BaseObject.Type.Building;
                        if ((s = CitySlot.GetRandomSlotOfType(replaceType)).IsNull)
                            return false;
                        break;
                    case BaseObject.Type.NukeLauncher:
                        replaceType = BaseObject.Type.Building;

                        if ((s = CitySlot.GetRandomSlotOfType(replaceType)).IsNull)
                            replaceType = BaseObject.Type.Sam;
                        if ((s = CitySlot.GetRandomSlotOfType(replaceType)).IsNull)
                            return false;
                        break;
                    case BaseObject.Type.ShieldGenerator:
                        replaceType = BaseObject.Type.Building;
                        if ((s = CitySlot.GetRandomSlotOfType(replaceType)).IsNull)
                            replaceType = BaseObject.Type.Sam;
                        if ((s = CitySlot.GetRandomSlotOfType(replaceType)).IsNull)
                            return false;
                        break;
                }
                
                if (s.IsNull)
                    return false;
                s.occupant.GetReplaced();
                s.SetOccupant(((BO_Static)manager.manager.SpawnObjectFromPool(typeToBuild, s.position)));
                return true;
            }
            return false;
        }
    }

}
