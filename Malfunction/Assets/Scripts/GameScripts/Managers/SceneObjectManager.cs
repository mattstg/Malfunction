using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour {

    public Transform objectPoolParent;
    public Transform activeObjectParent;
    
    public PrefabMap prefabs;
    public SpawnManager spawnManager;
    public GameManager manager;

    public static int emergencyPoolFillQuantity = 10;

    //updates all active objects
    BinaryQueues activeObjectQueues;

    //holds links to the head transform of each type of active object and object pool transforms
    Dictionary<BaseObject.Type, Transform> activeObjectParentDict = new Dictionary<BaseObject.Type, Transform>();
    Dictionary<BaseObject.Type, ObjectPool> objectPoolDictionary = new Dictionary<BaseObject.Type, ObjectPool>();

    public HashSet<Transform> activeAsteroids = new HashSet<Transform>();
    public HashSet<Transform> activeBuildings = new HashSet<Transform>();

    public void Initialize(GameManager newManager)
    {
        manager = newManager;
        activeObjectQueues = new BinaryQueues();
        activeObjectQueues.AddObjectToUpdate(prefabs.planet);
        InitializeStorageObjects();
        spawnManager.Initialize();
    }

    public void Refresh(float dt)
    {
        spawnManager.Refresh(dt);
        activeObjectQueues.Refresh(dt);
    }

    public void StartGame()
    {
        spawnManager.StartGame();
    }

    public void EndGame()
    {
        spawnManager.EndGame();
        activeObjectQueues.EndGame();
        activeAsteroids.Clear();
        activeBuildings.Clear();
    }

    public void AddObjectToUpdateQueue(BaseObject bo)
    {
        activeObjectQueues.AddObjectToUpdate(bo);
    }

    private void InitializeStorageObjects()
    {
        objectPoolDictionary.Clear();
        activeObjectParentDict.Clear();
        
        DeclareBaseObjectType(BaseObject.Type.Asteroid, prefabs.asteroid);
        DeclareBaseObjectType(BaseObject.Type.Building, prefabs.building);
        DeclareBaseObjectType(BaseObject.Type.Explosion, prefabs.explosion);
        DeclareBaseObjectType(BaseObject.Type.Rocket, prefabs.rocket);
        DeclareBaseObjectType(BaseObject.Type.Sam, prefabs.sam);
        DeclareBaseObjectType(BaseObject.Type.Nuke, prefabs.nuke);
        DeclareBaseObjectType(BaseObject.Type.NukeLauncher, prefabs.nukeLauncher);
        DeclareBaseObjectType(BaseObject.Type.NukeExplosion, prefabs.nukeExplosion);
    }

    private void DeclareBaseObjectType(BaseObject.Type type, BaseObject prefab) 
    {
        ObjectPool newPool = new ObjectPool(this,type,prefab);
        objectPoolDictionary.Add(type, newPool);

        GameObject activeObjs = new GameObject("Active " + type.ToString() + "s");
        activeObjs.transform.parent = activeObjectParent;
        activeObjectParentDict.Add(type, activeObjs.transform);
    }

    public BaseObject SpawnObjectFromPool(BaseObject.Type type, Vector2 position)
    {
        BaseObject toRet = objectPoolDictionary[type].Pop();
        toRet.transform.parent = activeObjectParentDict[type];
        toRet.Spawn(position);
        return toRet;
    }

    public void AddObjectToPool(BaseObject bo)
    {
        objectPoolDictionary[bo.type].Push(bo);
    }

    private class ObjectPool
    {
        private Transform transformParent;
        private SceneObjectManager manager;
        private Stack<BaseObject> stack;
        public BaseObject.Type type { private set; get; }
        public BaseObject referenceObject { private set; get; }
        
        public bool NotEmpty => stack.Count > 0;

        public ObjectPool(SceneObjectManager newManager, BaseObject.Type newType, BaseObject newReferenceObject)
        {
            manager = newManager;
            type = newType;
            referenceObject = newReferenceObject;

            transformParent = new GameObject(type.ToString() + "s").transform;
            transformParent.parent = manager.objectPoolParent.transform;
            
            stack = new Stack<BaseObject>();
            for (int i = 0; i < referenceObject.pooledQuantity; i++)
            {
                BaseObject newBO = referenceObject.CreateCopy();
                newBO.Initialize(manager);
                Push(newBO);
            }

            referenceObject.gameObject.SetActive(false);
        }

        public void Push(BaseObject bo)
        {
            stack.Push(bo);
            bo.transform.parent = transformParent;
        }

        public BaseObject Pop()
        {
            if(stack.Count == 0)
            {
                for (int i = 0; i < emergencyPoolFillQuantity; i++)
                {
                    BaseObject newBO = referenceObject.CreateCopy();
                    newBO.Initialize(manager);
                    Push(newBO);
                }
            }
            return stack.Pop();
        }
    }

    public BaseObject TransformToBaseObjext(Transform input)
    {
        return activeObjectQueues.TransformToBaseObject(input);
    }
}
