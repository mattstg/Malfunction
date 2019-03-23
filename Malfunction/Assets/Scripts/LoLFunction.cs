using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoLFunction  {
    public static char[] coefficentVarNames = new char[] {'x','y','z',' '};
    public int currentLevel;
    public int[] coefficents = new int[4];
    public int[] inputVars = new int[3];

    public int Solve()
    {
        return inputVars[0]*coefficents[0] + inputVars[1] * coefficents[1] + inputVars[2] * coefficents[2] + coefficents[3];
    }

    public override string ToString()
    {
        string toOut = "f" + GV.OutputSampleInputVariables(inputVars, false) + " = " + coefficents[0].ToString() + coefficentVarNames[0].ToString();
        //int digits = GV.GetNumberOfDigitsFromDifficulty(difficultyLevel);
        for(int i = 1; i < 4; ++i)
        {
            if (coefficents[i] != 0)
            {
                if (coefficents[i] > 0)
                {
                    toOut += " + " + coefficents[i].ToString() + coefficentVarNames[i].ToString();
                }
                else
                {
                    toOut += " − " + Mathf.Abs(coefficents[i]) + coefficentVarNames[i].ToString();
                }
            }
        }
        return toOut;
    }

    public string DebugOutput()
    {
        //return inputVars[0]*coefficents[0] + inputVars[1] * coefficents[1] + inputVars[2] * coefficents[2] + coefficents[3];
        return string.Format("Level{0}, Function:{1}, Inputs:{2}, Ans:{3}", currentLevel, ToString(), GV.OutputSampleInput(inputVars),Solve());
    }

    public static LoLFunction GenerateLoLFunction(int difficultLevel)
    {
        return new LoLFunction()
        {
            coefficents = GenerateLoLFunctionCoefficents(difficultLevel),
            inputVars = GenerateLoLInputVars(difficultLevel),
            currentLevel = difficultLevel
        };        
    }

    private static int[] GenerateLoLFunctionCoefficents(int currentLevel)
    {
        int numOfCoefficents = Mathf.Min(3,Mathf.Max(1, (int)GameFlow.uiLinks.numberOfCoefficents.Evaluate(currentLevel)));
        int[] coefficents = new int[4] { 0, 0, 0, 0 };
        for(int i = 0; i < numOfCoefficents;i++)
            coefficents[i] = GetValue(RangeType.coefficent, currentLevel);
        coefficents[3] = GetValue(RangeType.constant, currentLevel);
        return coefficents;
    }

    private static int[] GenerateLoLInputVars(int currentLevel)
    {
        int numOfInput = Mathf.Min(3, Mathf.Max(1, (int)GameFlow.uiLinks.numberOfCoefficents.Evaluate(currentLevel)));
        int[] inputVars = new int[3] { 0, 0, 0 };
        for (int i = 0; i < numOfInput; i++)
            inputVars[i] = GetValue(RangeType.inputVar, currentLevel);
        return inputVars;
    }

    public enum RangeType { inputVar, coefficent, constant}

    private static int GetValue(RangeType rt, int currentLevel, bool firstPass = true) //else is upper range
    {
        int toRet;
        switch (rt)
        {
            case RangeType.inputVar:
                toRet = (int)(Random.Range(
                    -GameFlow.uiLinks.inputvarLowerRange.Evaluate(currentLevel),
                    GameFlow.uiLinks.inputvarUpperRange.Evaluate(currentLevel)));
                break;
            case RangeType.coefficent:
                toRet = (int)(Random.Range(
                    -GameFlow.uiLinks.coefficentLowerRange.Evaluate(currentLevel),
                    GameFlow.uiLinks.coefficentUpperRange.Evaluate(currentLevel)));
                break;
            case RangeType.constant:
                if (GameFlow.uiLinks.chanceOfConstant.Evaluate(currentLevel) >= Random.value)
                    toRet = (int)(Random.Range(
                        -GameFlow.uiLinks.coefficentLowerRange.Evaluate(currentLevel),
                        GameFlow.uiLinks.coefficentUpperRange.Evaluate(currentLevel)));
                else
                    return 0;
                break;
            default:
                Debug.Log("Unhandled switch " + rt);
                toRet = Random.Range(1, 6);
                break;
        }
        if (toRet != 0)
            return toRet;

        if (toRet == 0 && firstPass)
            toRet = GetValue(rt, currentLevel,false);

        if (toRet == 0)
            return Random.Range(1, 6);
        else
            return toRet;
    }

    //private static int GetUpperRange(int difficultLevel)
    //{
    //    return 2 + (int)(difficultLevel*GV.difficultyModifier);
    //}
    //
    //private static int GetLowerRange(int difficultLevel)
    //{
    //    return -(int)(difficultLevel*GV.difficultyModifier) / 2;
    //}

}
