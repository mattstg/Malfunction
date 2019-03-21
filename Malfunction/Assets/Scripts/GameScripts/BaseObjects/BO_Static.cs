using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BO_Static : BaseObject
{
    private bool wasRemoved = false;
    public SpawnManager.CitySlot citySlot { private set; get; }

    public override BaseObject CreateCopy()
    {
        return base.CreateCopy();
    }

    public override void Despawn()
    {
        base.Despawn();
        if (!wasRemoved)
        {
            manager.spawnManager.city.PushSlot(citySlot);
            manager.activeBuildings.Remove(transform);
            wasRemoved = true;
        }
    }

    public override void ExternalDeath()
    {
        base.ExternalDeath();
        if (!wasRemoved)
        {
            manager.spawnManager.city.PushSlot(citySlot);
            manager.activeBuildings.Remove(transform);
            wasRemoved = true;
        }
        
    }

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
    }

    public override void InternalDeath()
    {
        base.InternalDeath();
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);
    }

    public virtual void AssignCitySlot(SpawnManager.CitySlot slot)
    {
        citySlot = slot;
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        citySlot = new SpawnManager.CitySlot(-1, Vector3.zero);
        manager.activeBuildings.Add(transform);
        wasRemoved = false;
    }
    
}
