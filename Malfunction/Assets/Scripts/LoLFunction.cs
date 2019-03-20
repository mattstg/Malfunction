using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoLFunction  {
    public static char[] coefficentVarNames = new char[] {'x','z','w' };
    public int difficultyLevel;
    public int[] coefficents = new int[4];
    public int[] inputVars = new int[4];
    bool hasConstant;

    public int Solve()
    {
        return inputVars[0]*coefficents[0] + inputVars[1] * coefficents[1] + inputVars[2] * coefficents[2] + coefficents[3];
    }

    public override string ToString()
    {
        string toOut = "";
        int digits = GV.GetNumberOfDigitsFromDifficulty(difficultyLevel);
        for(int i = 0; i < digits; ++i)
        {
            if (i != 0 && coefficents[i] >= 0)
                toOut += " + ";
           toOut += coefficents[i].ToString() + coefficentVarNames[i].ToString();
        }
        if (hasConstant)
            toOut += " + " + coefficents[3];
        return toOut;
    }

    public static LoLFunction GenerateLoLFunction(int difficultLevel)
    {
        bool _hasConstant = (Random.value > .5f);
        return new LoLFunction()
        {
            hasConstant = _hasConstant,
            coefficents = GV.CleanseForDifficultyLevel(GenerateLoLFunctionCoefficents(difficultLevel, _hasConstant), difficultLevel),
            inputVars = GV.CleanseForDifficultyLevel(GenerateLoLFunctionCoefficents(difficultLevel, _hasConstant), difficultLevel),
            difficultyLevel = difficultLevel
        };        
    }

    private static int[] GenerateLoLFunctionCoefficents(int difficultLevel, bool hasConstant)
    {
        int[] i = new int[4];
        i[0] = Random.Range(GetLowerRange(difficultLevel), GetUpperRange(difficultLevel));
        i[1] = Random.Range(GetLowerRange(difficultLevel), GetUpperRange(difficultLevel));
        i[2] = Random.Range(GetLowerRange(difficultLevel), GetUpperRange(difficultLevel));
        i[3] = (hasConstant) ? Random.Range(GetLowerRange(difficultLevel), GetUpperRange(difficultLevel)) :0;
        return i;
    }

    private static int GetUpperRange(int difficultLevel)
    {
        return 2 + (int)(difficultLevel*GV.difficultyModifier);
    }

    private static int GetLowerRange(int difficultLevel)
    {
        return -(int)(difficultLevel*GV.difficultyModifier) / 2;
    }

}
