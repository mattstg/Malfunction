using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : BaseObject {
    float clock = 0;
    public float rocketTime = 5;

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = Random.Range(0, rocketTime);
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);

        clock += dt;

        if(clock > rocketTime)
        {
            clock = 0;
            Rocket rocket = (Rocket) manager.SpawnObjectFromPool(Type.Rocket, transform.position);
            rocket.SetVelocity();
        }
    }
}
