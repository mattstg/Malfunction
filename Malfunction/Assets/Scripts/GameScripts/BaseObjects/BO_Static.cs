using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BO_Static : BaseObject
{
    SpawnManager.CitySlot citySlot;

    public override BaseObject CreateCopy()
    {
        return base.CreateCopy();
    }

    public override void Despawn()
    {
        base.Despawn();
        manager.spawnManager.city.PushSlot(citySlot);
        manager.activeBuildings.Remove(transform);
    }

    public override void ExternalDeath()
    {
        base.ExternalDeath();
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
        manager.activeBuildings.Add(transform);
    }
    
}
