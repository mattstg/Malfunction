using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV : MonoBehaviour {

	public static bool Sound_Active = true;
    public static MainScript ms;


    //public static int[] GetSampleRandomInput(int difficultyLevel)
    //{
    //
    //    return CleanseForDifficultyLevel(new int[] { Random.Range(0, 6), Random.Range(0, 6), Random.Range(0, 6) }, difficultyLevel);
    //}

    //public static int[] CleanseForDifficultyLevel(int[] v3, int difficultyLevel)
    //{
    //    if (difficultyLevel < difficultyLevelInputIncrease[1])
    //        v3[2] = 0;
    //    if (difficultyLevel < difficultyLevelInputIncrease[0])
    //        v3[1] = 0;
    //    return v3;
    //}

    public static string OutputSampleInput(int[] v3)
    {
        string toOut = OutputSampleInputVariables(v3, true) + " = ";
        string values = v3[0].ToString();
        bool onlyOne = true;
        for (int i = 1; i < 3; ++i)
        {
            if (v3[i] != 0)
            {
                values += ", ";
                values += v3[i].ToString();
                onlyOne = false;
            }
        }
        if (!onlyOne)
            values = "(" + values + ")";
        toOut += values;
        return toOut;
    }

    public static string OutputSampleInputVariables(int[] v3, bool noParensForOnlyX)
    {
        string toOut = LoLFunction.coefficentVarNames[0].ToString();
        bool onlyOne = noParensForOnlyX;
        for (int i = 1; i < 3; ++i)
        {
            if (v3[i] != 0)
            {
                toOut += ", ";
                toOut += LoLFunction.coefficentVarNames[i];
                onlyOne = false;
            }
        }
        if (!onlyOne)
            toOut = "(" + toOut + ")";
        return toOut;
    }
    
    public static string SecondsToTimeString(float t)
    {
        int hours = (int)(t / 3600);
        int minutes = ((int)(t / 60) % 60);
        int seconds = (int)(t % 60);
        string result = minutes + "m : " + seconds + "s";
        if (hours > 0)
            result = hours + "h : " + result;
        return result;
    }

    //public static int GetNumberOfDigitsFromDifficulty(int difficultyLevel)
    //{
    //    if (difficultyLevel >= difficultyLevelInputIncrease[1])
    //        return 3;
    //    if (difficultyLevel >= difficultyLevelInputIncrease[0])
    //        return 2;
    //    return 1;
    //}

}
