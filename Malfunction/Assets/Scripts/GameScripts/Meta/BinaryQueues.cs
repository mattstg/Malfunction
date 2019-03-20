using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryQueues  {

    Queue<BaseObject>[] queues;
    private int mainQueue = 0;
    private Queue<BaseObject> MainQueue => queues[mainQueue];
    private Queue<BaseObject> AuxQueue => queues[(mainQueue == 0) ? 1 : 0];

    private Dictionary<Transform, BaseObject> activeObjectDict = new Dictionary<Transform, BaseObject>();

    public BinaryQueues()
    {
        queues = new Queue<BaseObject>[] { new Queue<BaseObject>(), new Queue<BaseObject>() };
    }

    public void Refresh(float dt)
    {
        while(MainQueue.Count > 0)
        {
            BaseObject workingObject = MainQueue.Dequeue();
            if (workingObject.IsActive)
            {
                workingObject.Refresh(dt);
                AuxQueue.Enqueue(workingObject);
            }
            else
            {
                workingObject.Despawn();
                activeObjectDict.Remove(workingObject.transform);;
            }
        }

        mainQueue = (mainQueue == 0) ? 1 : 0;
        AuxQueue.Clear();
    }

    public void AddObjectToUpdate(BaseObject baseObject)
    {
        MainQueue.Enqueue(baseObject);
        activeObjectDict.Add(baseObject.transform, baseObject);
    }

    public bool Contains(BaseObject baseObj)
    {
       return Contains(baseObj.transform);
    }

    public bool Contains(Transform transform)
    {
        return activeObjectDict.ContainsKey(transform);
    }

    public BaseObject TransformToBaseObject(Transform transform)
    {
        return activeObjectDict[transform];
    }
}
