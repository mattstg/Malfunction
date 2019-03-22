using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : BO_Raycast
{
    public float flatLifespan = 6f;
    public float additionalLifespanMax = 4f;

    protected float lifespan;
    protected float clock = 0;

    public virtual Type TypeToSpawnOnDeath => Type.Explosion;

    Transform targetAsteroid;

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
        layerMask = LayerMask.GetMask( "Asteroids", "Explosions" );
    }

    public override void Refresh(float dt)
    {
        velocity += velocity.normalized * BuffManager.rocketAcceleration * dt;

        if (targetAsteroid != null)
        {
            if (!manager.activeAsteroids.Contains(targetAsteroid))
            {
                targetAsteroid = null;
            }
            else
            {
                velocity = velocity.magnitude * (targetAsteroid.position - transform.position).normalized;
                transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, velocity));
            }
        }

        base.Refresh(dt);
        clock += dt;
        if(clock > lifespan)
        {
            isActive = false;
            gameObject.SetActive(false);
            manager.SpawnObjectFromPool(TypeToSpawnOnDeath, gameObject.transform.position);
        }
    }

    public override void InternalCollisionDeath()
    {
        manager.SpawnObjectFromPool(TypeToSpawnOnDeath, deathRay.point);
        base.InternalCollisionDeath();
    }

    public override void SetVelocity()
    {
        //float random = Random.Range(-manager.spawnManager.cityXSize, manager.spawnManager.cityXSize);
        //Vector3 point = new Vector3(manager.spawnManager.planetFloor.position.x + random, manager.spawnManager.planetFloor.position.y, 0);
        velocity = Vector2.up * startVelocity;
    }

    public virtual void SetTarget(Transform newTarget)
    {
        targetAsteroid = newTarget;
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = 0;
        lifespan = flatLifespan + additionalLifespanMax * Random.Range(0f, 1f);
    }

    public override void DeathFromExplosion()
    {
        base.DeathFromExplosion();
        manager.SpawnObjectFromPool(TypeToSpawnOnDeath, transform.position);
    }
}
