using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BO_Raycast : BaseObject {
    
    public float startVelocity = 2;
    protected Vector3 velocity;

    protected RaycastHit2D deathRay;
    protected int layerMask = 0;
    
    
    public override void Refresh(float dt)
    {
        base.Refresh(dt);
        if (!TestForCollision(dt))
            transform.position = new Vector3(transform.position.x + velocity.x * dt, transform.position.y + velocity.y * dt, 0);
        else
        {
            if (isActive == true)
            {
                InternalDeath();
                isActive = false;
                gameObject.SetActive(false);
            }
        }
    }

    public override void InternalDeath()
    {
        deathRay = new RaycastHit2D();
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        SetVelocity();
    }

    public virtual void SetVelocity()
    {
        
    }

    public virtual bool TestForCollision(float dt)
    {
        deathRay = Physics2D.Raycast(transform.position, velocity.normalized, velocity.magnitude * dt, layerMask);
        return deathRay.transform != null;
    }
}

