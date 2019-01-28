using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour {

	public void MakeSound(string soundToMake)
    {
        LOLAudio.Instance.PlayAudio(soundToMake);
    }
}
