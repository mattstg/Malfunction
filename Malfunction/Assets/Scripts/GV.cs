using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV : MonoBehaviour {

	public static bool Sound_Active = true;
    public static MainScript ms;
    [HideInInspector] public static Vector2Int difficultyLevelInputIncrease = new Vector2Int(5, 12);


    public static Vector3Int GetSampleRandomInput(int difficultyLevel)
    {
        return CleanseForDifficultyLevel(new Vector3Int(Random.Range(0, 6), Random.Range(0, 6), Random.Range(0, 6)), difficultyLevel);
    }

    public static Vector3Int CleanseForDifficultyLevel(Vector3Int v3, int difficultyLevel)
    {
        if (difficultyLevel < difficultyLevelInputIncrease[1])
            v3[2] = 0;
        if (difficultyLevel < difficultyLevelInputIncrease[0])
            v3[1] = 0;
        return v3;
    }

    public static string OutputSampleInput(Vector3Int v3, int difficultyLevel)
    {
        string toOut = "(";
        int digits = GetNumberOfDigitsFromDifficulty(difficultyLevel);
        for (int i = 0; i < digits; ++i)
        {
            if (i != 0)
                toOut += ",";
            toOut += v3[i].ToString();
        }
        toOut += ")";
        Debug.Log("TOUT: " + toOut);
        return toOut;
    }

    public static int GetNumberOfDigitsFromDifficulty(int difficultyLevel)
    {
        if (difficultyLevel >= difficultyLevelInputIncrease[1])
            return 3;
        if (difficultyLevel >= difficultyLevelInputIncrease[0])
            return 2;
        return 1;
    }

}
