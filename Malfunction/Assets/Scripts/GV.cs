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
        string toOut = "(" + v3[0];
        for (int i = 1; i < 3; ++i)
        {
            if (v3[i] != 0)
            {
                toOut += ",";
                toOut += v3[i].ToString();
            }
        }
        toOut += ")";
        return toOut;
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
