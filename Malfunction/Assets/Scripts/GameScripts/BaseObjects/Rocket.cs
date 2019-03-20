using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : BO_Raycast
{
    public float flatLifespan = 1.5f;
    public float additionalLifespanMax = 2.5f;

    protected float lifespan;
    protected float clock = 0;

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
        layerMask = LayerMask.GetMask("Asteroids", "Explosions");
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);
        clock += dt;
        if(clock > lifespan)
        {
            isActive = false;
            gameObject.SetActive(false);
            manager.SpawnObjectFromPool(Type.Explosion, gameObject.transform.position);
        }
    }

    public override void InternalDeath()
    {
        manager.SpawnObjectFromPool(Type.Explosion, deathRay.point);
        base.InternalDeath();
    }

    public override void SetVelocity()
    {
        //float random = Random.Range(-manager.spawnManager.cityXSize, manager.spawnManager.cityXSize);
        //Vector3 point = new Vector3(manager.spawnManager.planetFloor.position.x + random, manager.spawnManager.planetFloor.position.y, 0);
        velocity = Vector2.up * startVelocity;
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = 0;
        lifespan = flatLifespan + additionalLifespanMax * Random.Range(0f, 1f);
    }

    public override void ExternalDeath()
    {
        base.ExternalDeath();
        manager.SpawnObjectFromPool(Type.Explosion, transform.position);
    }
}
