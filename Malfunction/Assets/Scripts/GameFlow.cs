using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : Flow
{
    LevelPkg currentLevelPkg;
    UIGameLinks uiLinks;

    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.ClearDisabledSounds();
        uiLinks = GameObject.FindObjectOfType<UIGameLinks>();
        initialized = true;
        currentLevelPkg = LevelPkg.GenerateLevelPackage(0);

    }

    public override void Update(float dt)
    {
        if (initialized)
        {
                     
        }
    }

    public void ConfirmButtonPressed()
    {

    }
}
