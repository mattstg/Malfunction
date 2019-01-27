using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoLFunction  {
    public static char[] coefficentVarNames = new char[] {'x','z','w' };
    public Vector3Int coefficents;

    public int Solve(Vector3Int vars)
    {
        return vars[0]*coefficents[0] + vars[1] * coefficents[1] + vars[2] * coefficents[2];
    }

    public string ToString(int difficultLevel)
    {
        string toOut = "";
        int digits = GV.GetNumberOfDigitsFromDifficulty(difficultLevel);
        for(int i = 0; i < digits; ++i)
        {
            if (i != 0 && coefficents[i] >= 0)
                toOut += " + ";
           toOut += coefficents[i].ToString() + coefficentVarNames[i].ToString();
        }
        return toOut;
    }

    public static LoLFunction GenerateLoLFunction(int difficultLevel)
    {
        return new LoLFunction()
        {
            coefficents = GV.CleanseForDifficultyLevel(GetCoefficentRange(difficultLevel), difficultLevel)
        };        
    }

    private static Vector3Int GetCoefficentRange(int difficultLevel)
    {
        int lowerRange = (difficultLevel > 4) ? -3 : 0;
        return new Vector3Int(Random.Range(lowerRange, GetUpperRange(difficultLevel)), Random.Range(lowerRange, GetUpperRange(difficultLevel)), Random.Range(lowerRange, GetUpperRange(difficultLevel)));
    }

    private static int GetUpperRange(int difficultLevel)
    {
        return 2 + (int)(difficultLevel);
    }

}
