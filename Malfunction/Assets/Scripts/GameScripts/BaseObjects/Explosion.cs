using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseObject
{
    public float duration = 1;
    public Vector2 sizeRange = new Vector2(1f, 3f);

    private Vector3[] sizes;

    private float clock = 0;
    private ContactFilter2D contactFilter;

    public override BaseObject CreateCopy()
    {
        return base.CreateCopy();
    }

    public override void InternalDeath()
    {
        base.InternalDeath();
    }

    public override void Despawn()
    {
        base.Despawn();
    }

    public override void Initialize(SceneObjectManager newManager)
    {
        base.Initialize(newManager);
        sizes = new Vector3[] { new Vector3(sizeRange.x, sizeRange.x, sizeRange.x), new Vector3(sizeRange.y, sizeRange.y, sizeRange.y) };
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Buildings", "Asteroids", "Rockets"));
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
                        result.ExternalDeath();
                        break;
                    case Type.Asteroid:
                        result.ExternalDeath();
                        break;
                    case Type.Rocket:
                        result.ExternalDeath();
                        break;
                    default:
                        break;
                }
                indexPointer++;
            }
        }
    }
}
