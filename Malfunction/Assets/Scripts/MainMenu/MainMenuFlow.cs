using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.AddDisabledSound(LOLAudio.collectRain);
        LOLAudio.Instance.AddDisabledSound(LOLAudio.land);

        UnityEngine.GameObject.FindObjectOfType<MainMenu>().SDKLoaded(progressNumber);
        
        initialized = true;
    }

    public override void Update(float dt)
    {
        Debug.Log("yo");
    }
}
