using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        UnityEngine.GameObject.FindObjectOfType<MainMenu>().SDKLoaded(progressNumber);
        
        initialized = true;
    }
}
