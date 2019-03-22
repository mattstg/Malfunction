using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : BO_Raycast
{

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
        layerMask = LayerMask.GetMask("Buildings", "Planet", "Explosions", "Shield");
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        manager.activeAsteroids.Add(transform);
    }

    public override void UpdateQueueRemove()
    {
        base.UpdateQueueRemove();
        manager.activeAsteroids.Remove(transform);
    }

    public override void InternalCollisionDeath()
    {
        manager.SpawnObjectFromPool(Type.Explosion, deathRay.point);
        base.InternalCollisionDeath();
        manager.activeAsteroids.Remove(transform);
    }
    
    public override void SetVelocity()
    {
        float random = Random.Range(-manager.spawnManager.citySize, manager.spawnManager.citySize);
        Vector3 point = new Vector3(manager.spawnManager.planetFloor.position.x + random, manager.spawnManager.planetFloor.position.y, 0);
        velocity = (point - transform.position).normalized * startVelocity;
    }

    public override void DeathFromExplosion()
    {
        base.DeathFromExplosion();
        manager.SpawnObjectFromPool(Type.Explosion, transform.position);
    }
}
