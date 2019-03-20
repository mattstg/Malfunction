using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : BaseObject
{
    public override BaseObject CreateCopy()
    {
        return null;
    }

    public override void InternalDeath()
    {
        
    }

    public override void Despawn()
    {
        
    }

    public override void ExternalDeath()
    {
        
    }

    public override void Initialize(SceneObjectManager newManager)
    {
        manager = newManager;
        isActive = true;
        gameObject.SetActive(true);
    }

    public override void Refresh(float dt)
    {
        
    }

    public override void Spawn(Vector2 posistion)
    {
        
    }
}
