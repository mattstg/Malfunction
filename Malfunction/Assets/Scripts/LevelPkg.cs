using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPkg  {

    public LoLFunction lolFunc;

    public Vector3Int[] sampleInputs;
    public int[] sampleOutputs;
    public LoLFunction[] functionOptions;
    public int ansIndex;
    public Vector3Int inputToSolve;
    public int outputToSolve;
    



    public static LevelPkg GenerateLevelPackage(int difficultyLevel)
    {
        //return null;
        LevelPkg lvlpkg = new LevelPkg()
        {
            lolFunc = LoLFunction.GenerateLoLFunction(difficultyLevel),
            sampleInputs = new Vector3Int[] 
            {
                GV.CleanseForDifficultyLevel(new Vector3Int(1, 1, 1),difficultyLevel),
                GV.CleanseForDifficultyLevel(new Vector3Int(2, 2, 2),difficultyLevel),
                GV.CleanseForDifficultyLevel(new Vector3Int(3, 3, 3),difficultyLevel)
            },
            functionOptions = new LoLFunction[] { LoLFunction.GenerateLoLFunction(difficultyLevel), LoLFunction.GenerateLoLFunction(difficultyLevel), LoLFunction.GenerateLoLFunction(difficultyLevel) },
            ansIndex = 0,
            inputToSolve = new Vector3Int(5, 5, 5)
        };
        lvlpkg.outputToSolve = lvlpkg.lolFunc.Solve(lvlpkg.inputToSolve);
        return lvlpkg;
    }
}
