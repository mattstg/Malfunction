using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseObject
{
    public static Vector2 yBounds;
    public float duration = 1;
    public Vector2 sizeRange = new Vector2(1f, 3f);

    public static float yVelo = 0.2f;
    public static float randomXMax = 0.2f;

    public Vector2 velo = Vector2.zero;

    private Vector3[] sizes;

    private float clock = 0;
    private ContactFilter2D contactFilter;

    public override BaseObject CreateCopy()
    {
        return base.CreateCopy();
    }

    public override void InternalCollisionDeath()
    {
        base.InternalCollisionDeath();
    }

    public override void UpdateQueueRemove()
    {
        base.UpdateQueueRemove();
    }

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
        sizes = new Vector3[] { new Vector3(sizeRange.x, sizeRange.x, sizeRange.x), new Vector3(sizeRange.y, sizeRange.y, sizeRange.y) };
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Buildings", "Asteroids"));
        contactFilter.ClearDepth();
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);
        if(clock >= duration)
        {
            isActive = false;
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + velo.x * dt, transform.position.y + velo.y * dt, 0);
            clock += dt;
            SetScale();
            TestCollisions();
        }
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = 0;
        SetScale();
        velo = new Vector2(Random.Range(-randomXMax, randomXMax), yVelo);
    }

    public void SetScale()
    {
        transform.localScale = Vector3.Slerp(sizes[0], sizes[1], clock/duration);
    }

    public void TestCollisions()
    {
        Collider2D[] results = new Collider2D[5];
        coli.OverlapCollider(contactFilter, results);
        
        int indexPointer = 0;
        if (results.Length > 0)
        {
            while(results[indexPointer] != null)
            {
                BaseObject result = manager.TransformToBaseObjext(results[indexPointer].transform);
                switch (result.type)
                {
                    case Type.Building:
                        result.DeathFromExplosion();
                        break;
                    case Type.Asteroid:
                        result.DeathFromExplosion();
                        break;
                    case Type.Rocket:
                        result.DeathFromExplosion();
                        break;
                    case Type.Sam:
                        result.DeathFromExplosion();
                        break;
                    case Type.NukeLauncher:
                        result.DeathFromExplosion();
                        break;
                    case Type.Nuke:
                        result.DeathFromExplosion();
                        break;
                    case Type.ShieldGenerator:
                        result.DeathFromExplosion();
                        break;
                    default:
                        break;
                }
                indexPointer++;
            }
        }
    }
}
