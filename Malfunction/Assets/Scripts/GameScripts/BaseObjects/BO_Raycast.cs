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
        {
            transform.position = new Vector3(transform.position.x + velocity.x * dt, transform.position.y + velocity.y * dt, 0);
        }
        else
        {
            if (isActive == true)
            {
                InternalCollisionDeath();
                isActive = false;
                gameObject.SetActive(false);
            }
        }
    }

    public override void InternalCollisionDeath()
    {
        deathRay = new RaycastHit2D();
    }

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        SetVelocity();
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, velocity));
    }

    public virtual void SetVelocity()
    {
        
    }

    public virtual bool TestForCollision(float dt)
    {
        deathRay = Physics2D.Raycast(transform.position, velocity.normalized, velocity.magnitude * dt, layerMask);
        if (deathRay.transform != null && deathRay.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Shield")))
        {
            ((ShieldGenerator)manager.TransformToBaseObjext(deathRay.transform.parent)).TurnShieldOff();
        }
        return deathRay.transform != null;
    }
}

