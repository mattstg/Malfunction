﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBank
{
    #region
    private QuestionBank() { }

    private static QuestionBank instance;
    public static QuestionBank Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestionBank();
            }
            return instance;
        }
    }
    #endregion Singleton

    Stack<LoLFunction> questionBank;
    readonly int questionBankInitialSize = 500;
    public static bool debugMode = false;

    public LoLFunction Initialize()
    {
        //Initialize the question bank
        questionBank = new Stack<LoLFunction>();
        for (int i = questionBankInitialSize; i > 0; --i)
        {
            LoLFunction lf;
            if (i % 5 == 0 && i <= 25)
            {
                lf = LoLFunction.GenerateLoLFunction(i,true);
            }
            else
            {
                lf = LoLFunction.GenerateLoLFunction(i,false);
            }
            questionBank.Push(lf);
        }
        return questionBank.Pop();

    }

    public LoLFunction Pop()
    {
        //if (questionBank.Count <= 0)
        //    Debug.Log("Holy shit someone beat the game....");
        return questionBank.Pop();
    }
     


}