using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : Flow
{
    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.ClearDisabledSounds();
        initialized = true;
    }

    public override void Update(float dt)
    {
        if (initialized)
        {
                     
        }
    }
}
