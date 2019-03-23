using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBot {
    public static bool childBotActive = false;
    const float childAvrgResponse = 4;
    float childAvrgResponseIncrease = .1f;
    float childCorrectResponseProbability = .9f;
    float childCorrectResponseLossPerQuestion = .01f;
    float timeOfNextAttempt;
    float initialTime;
    
    public ChildBot()
    {
        initialTime = Time.time;
        timeOfNextAttempt = Time.time + childAvrgResponse;
    }

    public bool ShouldChildAttempt(int currentLevel)
    {
        Debug.Log("time of next solve: " + (timeOfNextAttempt- initialTime) + " vs current time " + (Time.time - initialTime));
        return Time.time >= timeOfNextAttempt;
    }
    // Use this for initialization
    public bool AttemptSolve(int currentLevel)
    {
        bool hasSolved = Random.value < (childCorrectResponseProbability - childCorrectResponseLossPerQuestion * currentLevel);
        if (hasSolved)
            timeOfNextAttempt = Time.time + childAvrgResponse + childAvrgResponseIncrease * currentLevel;
        else
            timeOfNextAttempt = Time.time + childAvrgResponse / 2 + childAvrgResponseIncrease * currentLevel;
        return hasSolved;
    }
}
