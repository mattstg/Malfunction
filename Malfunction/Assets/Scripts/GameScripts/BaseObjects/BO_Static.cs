using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BO_Static : BaseObject
{
    public SpawnManager.CitySlot citySlot { private set; get; }
    
    public override void UpdateQueueRemove()
    {
        PushCitySlot();
        base.UpdateQueueRemove();
        manager.activeBuildings.Remove(transform);
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
    
    public virtual void GetReplaced()
    {
        isActive = false;
        gameObject.SetActive(false);
        PushCitySlot();
    }

    private void PushCitySlot()
    {
        if (!citySlot.IsNull)
        {
            citySlot.EmptyOccupant();
            citySlot = SpawnManager.CitySlot.nullSlot;
        }
    }

    public override void DeathFromExplosion()
    {
        base.DeathFromExplosion();
    }
}
