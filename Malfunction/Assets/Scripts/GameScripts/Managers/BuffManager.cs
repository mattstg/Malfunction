using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager {
    public GameManager manager;

    public static float rocketAcceleration = 1; //0
    public static float turretFireTimePerc = .7f;

    public float maxRocketAcc = 3;
    public float turretLockOnBuff = 0.25f;

    public BuffManager(GameManager newMan)
    {
        manager = newMan;
        Reset();
    }

    public void Reset()
    {
        //rocketAcceleration = 0;
        //turretFireTimePerc = 1;
    }
}
