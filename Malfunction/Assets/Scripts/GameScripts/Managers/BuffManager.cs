using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager {
    public GameManager manager;

    public static float rocketAcceleration = 0;
    public static float turretFireTimePerc = 1;

    public int maxStreak = 25;
    public float maxRocketAcc = 3;
    public float turretLockOnBuff = 0.25f;

    private int _currentStreak = 0;
    private int currentStreak { set { _currentStreak = value; UpdateStreakText(); } get { return _currentStreak; } }

    public BuffManager(GameManager newMan)
    {
        manager = newMan;
        Reset();
    }

    public void Reset()
    {
        currentStreak = 0;
        rocketAcceleration = 0;
        turretFireTimePerc = 1;
    }

    public void SetStreak(int newS)
    {
        currentStreak = newS;
        rocketAcceleration = Mathf.Clamp01(currentStreak / maxStreak) * maxRocketAcc;
        turretFireTimePerc = 1 + Mathf.Clamp01(currentStreak / maxStreak) * turretLockOnBuff;
    }

    public void UpdateStreakText()
    {
        GameFlow.uiLinks.streakText.text = currentStreak.ToString();
    }
}
