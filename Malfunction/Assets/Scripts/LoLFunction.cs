using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoLFunction  {
    public static char[] coefficentVarNames = new char[] {'x','y','z',' '};
    public int currentLevel;
    public int[] coefficents = new int[4];
    public int[] inputVars = new int[3];
    public bool isDumbGraphFunction = false;
    public bool isGraphCoefficientInverse = false;

    public int Solve()
    {
        if (!isDumbGraphFunction)
            return inputVars[0] * coefficents[0] + inputVars[1] * coefficents[1] + inputVars[2] * coefficents[2] + coefficents[3];
        else
            return (isGraphCoefficientInverse) ? inputVars[0] / coefficents[0] + coefficents[3] : inputVars[0] * coefficents[0] + coefficents[3];
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

    public static LoLFunction GenerateLoLFunction(int difficultLevel, bool _isDumbGraphFunction)
    {
        if (!_isDumbGraphFunction)
        {
            return new LoLFunction()
            {
                coefficents = GenerateLoLFunctionCoefficents(difficultLevel),
                inputVars = GenerateLoLInputVars(difficultLevel),
                currentLevel = difficultLevel
            };
        }
        else
        {
            bool isInverse = (Random.Range(0, 2) == 1); // 50/50 coefficient = inverse: {1 or 1/2 or 1/3} or not inverse: {1 or 2 or 3}
            int coefficient = Random.Range(1, 4);   // 1 - 3
            int input;
            int offset = Random.Range(0, 5);    // 0 - 4
            int inputMax;
            // This if statement only works for specific arrangements of the values above.
            if (isInverse && coefficient != 1)
            {
                inputMax = (8 / coefficient);
                input = Random.Range(0, inputMax + 1) * coefficient;
            }
            else
            {
                inputMax = ((8 - offset) / coefficient);
                input = Random.Range(0, inputMax + 1);
            }
            int output = coefficient * input + offset;
            Debug.Log(string.Format("function: isInverse = {0}, coefficient = {1}, offset = {2}, inputMax = {3}, input = {4}, output = {5} : ({4}, {5})",
                isInverse, coefficient, offset, inputMax, input, output));
            if (input < 0 || input > 8 || output < 0 || output > 8)
                Debug.Log("Error. (" + input + ", " + output + ") : out of bounds.");
            return new LoLFunction()
            {
                isGraphCoefficientInverse = isInverse,
                coefficents = new int[] { coefficient, 0, 0, offset },
                inputVars = new int[] { input, 0, 0 },
                isDumbGraphFunction = true,
                currentLevel = difficultLevel
            };
        }        
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
