using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour {

    public Transform objectPoolParent;
    private Dictionary<BaseObject.Type, ObjectPool> objectPoolDictionary = new Dictionary<BaseObject.Type, ObjectPool>();
    public PrefabMap prefabs;
    public SpawnManager spawnManager;

    BinaryQueues activeObjects;


    public float timeBetweenAsteroids = 1;
    private float asteroidClock = 0;

    //active buildings? 

    public void Initialize()
    {
        activeObjects = new BinaryQueues();
        activeObjects.AddObjectToUpdate(prefabs.planet);
        FillObjectPools();
        spawnManager.Initialize();
    }

    public void Refresh(float dt)
    {
        activeObjects.Refresh(dt);

        if(asteroidClock > timeBetweenAsteroids)
        {
            asteroidClock = 0;
            float random = Random.Range(-spawnManager.cityXSize,spawnManager.cityXSize);
            Vector3 spawnPos = new Vector3(spawnManager.sky.position.x + random, spawnManager.sky.position.y, 0);
            SpawnObjectFromPool(BaseObject.Type.Asteroid, spawnPos);
        }
        else
        {
            asteroidClock += dt;
        }
    }

    public void AddObjectToUpdateQueue(BaseObject bo)
    {
        activeObjects.AddObjectToUpdate(bo);
    }

    private void FillObjectPools()
    {
        objectPoolDictionary.Clear();
        
        FillTypePool(BaseObject.Type.Asteroid, prefabs.asteroid);
        FillTypePool(BaseObject.Type.Building, prefabs.building);
        FillTypePool(BaseObject.Type.Explosion, prefabs.explosion);
        FillTypePool(BaseObject.Type.Rocket, prefabs.rocket);
        
    }

    private void FillTypePool(BaseObject.Type type, BaseObject prefab) 
    {
        ObjectPool newPool = new ObjectPool();
        objectPoolDictionary.Add(type, newPool);

        for (int i = 0; i < prefab.pooledQuantity; i++)
        {
            BaseObject newBO = prefab.CreateCopy();
            newBO.Initialize(this);
            newPool.Push(newBO);
        }

        prefab.gameObject.SetActive(false);
    }

    public BaseObject SpawnObjectFromPool(BaseObject.Type type, Vector2 position)
    {
        BaseObject toRet = null;
        /*
        switch (type)
        {
            case BaseObject.Type.Building:
                if (objectPoolDictionary[type].NotEmpty)
                {
                    toRet = objectPoolDictionary[type].Pop();
                }
                break;
            case BaseObject.Type.Asteroid:
                if (objectPoolDictionary[type].NotEmpty)
                {
                    toRet = objectPoolDictionary[type].Pop();
                }
                break;
        }*/

        toRet = objectPoolDictionary[type].Pop();

        toRet?.Spawn(position);
        return toRet;
    }

    public void AddObjectToPool(BaseObject bo)
    {
        objectPoolDictionary[bo.type].Push(bo);
    }

    private class ObjectPool
    {
        public Stack<BaseObject> stack;
        public BaseObject.Type type;

        public BaseObject Peek => stack.Peek();

        public bool NotEmpty => stack.Count > 0;

        public ObjectPool()
        {
            stack = new Stack<BaseObject>();
        }

        public void Push(BaseObject bo)
        {
            stack.Push(bo);
        }

        public BaseObject Pop()
        {
            return stack.Pop();
        }
    }

    public BaseObject TransformToBaseObjext(Transform input)
    {
        return activeObjects.TransformToBaseObject(input);
    }
}
