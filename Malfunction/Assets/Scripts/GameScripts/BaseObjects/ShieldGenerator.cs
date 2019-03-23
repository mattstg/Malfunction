using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : BO_Static {
    public Transform shield;

    float clock = 0;
    bool shieldOn = false;
    public static float timeToRegen = 4;

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = 0;
        shieldOn = true;
        shield.gameObject.SetActive(true);
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);

        if (!shieldOn)
        {
            clock += dt;
            if(clock > timeToRegen)
            {
                shieldOn = true;
                shield.gameObject.SetActive(true);
            }
        }
    }

    public void TurnShieldOff()
    {
        shieldOn = false;
        shield.gameObject.SetActive(false);
        clock = 0;
    }
}
