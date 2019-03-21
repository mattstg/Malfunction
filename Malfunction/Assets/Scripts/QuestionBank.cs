using System.Collections;
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

    Stack<LoLFunction> questionBank = new Stack<LoLFunction>();
    readonly int questionBankInitialSize = 200;

    public LoLFunction Initialize()
    {
        //Initialize the question bank
        for(int i = questionBankInitialSize; i > 0;--i)
            questionBank.Push(LoLFunction.GenerateLoLFunction(i));
        questionBank.Push(LoLFunction.GenerateLoLFunction(0));
        questionBank.Push(LoLFunction.GenerateLoLFunction(0));
        return LoLFunction.GenerateLoLFunction(0);

    }

    public LoLFunction Pop()
    {
        if (questionBank.Count <= 0)
            Debug.Log("Holy shit someone beat the game....");
        return questionBank.Pop();
    }
     


}