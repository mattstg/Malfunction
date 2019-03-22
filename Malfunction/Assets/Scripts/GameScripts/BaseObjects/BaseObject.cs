using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
    public enum Type { Null = 0, Building = 1, Asteroid = 2, Explosion = 3, Rocket = 4, Sam = 5, Nuke = 6, NukeLauncher = 7, NukeExplosion = 8, Planet = 9, ShieldGenerator  = 10}
    protected SceneObjectManager manager;

    public Collider2D coli;
    protected bool isActive = false;
    public bool IsActive => isActive;
    public Type type;
    public int pooledQuantity = 10;


	public virtual void Initialize(SceneObjectManager newManager)
    {
        manager = newManager;
        isActive = false;
        gameObject.SetActive(false);
    }

    public virtual void Refresh(float dt)
    {
        //returns where or not the baseObject is still alive
        
    }

    public virtual void Spawn(Vector2 posistion)
    {
        isActive = true;
        gameObject.SetActive(true);
        transform.position = (Vector3)posistion;
        manager.AddObjectToUpdateQueue(this);
    }

    public virtual void UpdateQueueRemove()
    {
        isActive = false;
        gameObject.SetActive(false);
        manager.AddObjectToPool(this);
    }

    public virtual BaseObject CreateCopy()
    {
        GameObject newGO = Instantiate(gameObject) as GameObject;
        BaseObject bo = newGO.GetComponent<BaseObject>();
        Initialize(manager);
        return bo;
    }

    public virtual void InternalCollisionDeath()
    {

    }

    public virtual void DeathFromExplosion()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
