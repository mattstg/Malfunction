using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBot {

    const float childAvrgResponse = 6;
    float childAvrgResponseIncrease = .1f;
    float childCorrectResponseProbability = .9f;
    float childCorrectResponseLossPerQuestion = .01f;
    float timeOfNextAttempt;
    
    public ChildBot()
    {
        timeOfNextAttempt = Time.time + childAvrgResponse;
    }

    public bool ShouldChildAttempt(int currentLevel)
    {
        Debug.Log("time of next solve: " + timeOfNextAttempt + " vs current time " + Time.time);
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
