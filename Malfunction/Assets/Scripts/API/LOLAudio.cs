﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;

[SerializeField] public enum SoundEffects { ButtonClick, PositiveFeedback, NegativeFeedback }

//This is one of the grossest classes ever, we salavaged it from one of our other titles and are in a hurry, do not judge us

public class LOLAudio
{
#region Singleton
    private static LOLAudio instance;

    public static LOLAudio Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LOLAudio();
            }
            return instance;
        }
    }
    #endregion

    public static readonly int soundPack = 1;   //0(1), 1(0):Kinda thundery and high pitch, 2(0): Super quiet, would need to lower music
    static readonly int numOfExplosionSounds = 8;  //0: 9, 1:6, 2:5
    static readonly int numOfNukeExplosionSounds = 3;
    
    public static readonly string bgMusicMainMenu = "bgMusic";
    //public static readonly string lightRain = "lightRain.mp3";
    //public static readonly string land = "land.wav";
    //public static readonly string collectRain = "raindrop.wav";
    //public static readonly string aphidHit = "aphidHit.wav";

    //List<string> disabledSounds;
    AudioSource musicPlayer;  //Plays music for non-lol
    
    AudioSource bgMusicPlayer;
    AudioClip   buttonClick;
    //AudioClip   correctFeedback;
    //AudioClip   incorrectFeedback;
    Dictionary<string, AudioClip> audioClips;
    

    private LOLAudio()
    {
        
    }

    public void Initialize()
    {
        //bgMusicPlayer = Resources.Load<AudioClip>("Music/land");
//#if UNITY_EDITOR
        buttonClick = Resources.Load<AudioClip>("Music/ButtonClick");
        //correctFeedback = Resources.Load<AudioClip>("Music/PositiveFeedback");
        //incorrectFeedback = Resources.Load<AudioClip>("Music/aphidHit");
        audioClips = new Dictionary<string, AudioClip>()
        {
            { "PositiveFeedback", Resources.Load<AudioClip>("Music/PositiveFeedback")},
            {"NegativeFeedback", Resources.Load<AudioClip>("Music/aphidHit") },
            {"ButtonClick", Resources.Load<AudioClip>("Music/ButtonClick") }
        };

        for (int i = 0; i < numOfExplosionSounds; i++)
            audioClips.Add("exp" + i, Resources.Load<AudioClip>("Music/" + soundPack + "/exp" + i));
        for (int i = 0; i < numOfNukeExplosionSounds; i++)
            audioClips.Add("nuke" + i, Resources.Load<AudioClip>("Music/nuke" + i));

        //#endif
        //PlayBackgroundAudio("bgMusic");
        //explosionExpires = new Queue<float>();

    }

    public void PlayBackgroundAudio(string _name)
    {
//#if UNITY_EDITOR
        if (!bgMusicPlayer)
        {
            GameObject go = new GameObject();
            go.name = "BgMusic";
            AudioSource audioSrc = go.AddComponent<AudioSource>();
            string filePath = "Music/" + System.IO.Path.GetFileNameWithoutExtension(_name);
            audioSrc.clip = Resources.Load<AudioClip>(filePath);
            audioSrc.loop = true;
            audioSrc.Play();
            bgMusicPlayer = audioSrc;
            Object.DontDestroyOnLoad(go);
        }
        else
        {
            bgMusicPlayer.Stop();
            string filePath = "Music/" + System.IO.Path.GetFileNameWithoutExtension(_name);
            bgMusicPlayer.clip = Resources.Load<AudioClip>(filePath);
            bgMusicPlayer.Play();
        }
/*#elif UNITY_WEBGL
        StopAudio("bgMusic0");
        StopAudio("bgMusic1");
        StopAudio("bgMusic2");
        StopAudio("bgMusic3");
        LOLSDK.Instance.PlaySound(_name, true, true);
#endif*/
    }

    public void PlayExplosion()
    {
        
        //if(explosionExpires.Count < maxExplosionSounds)
        //{
          //  explosionExpires.Enqueue(Time.time + 3f);
            int expIndex = Random.Range(0, numOfExplosionSounds);
            PlayAudio("exp" + expIndex);
       // Debug.Log("Play explosion called for: " + "exp" + expIndex);
        //}
    }

    public void PlayNukeExplosion()
    {
        int expIndex = Random.Range(0, numOfNukeExplosionSounds);
        PlayAudio("nuke" + expIndex);
        //Debug.Log("Play explosion called for: " + "nukeexp" + numOfNukeExplosionSounds);
    }

    public void SetBGLevel(float volume)
    {
//#if UNITY_EDITOR    
            bgMusicPlayer.volume = volume;
/*#elif UNITY_WEBGL
        LOLSDK.Instance.ConfigureSound(1, volume, 1); //Should be 0 or 1? who knows
#endif*/
    }
    
    public void PlayAudio(string _name, bool loop = false)
    {
        if (!GV.Sound_Active || !LOLSDK.Instance.IsInitialized)
            return;
//#if UNITY_EDITOR
        PlayEditorAudio(_name, loop);        
/*#elif UNITY_WEBGL
        LOLSDK.Instance.PlaySound(_name, false, loop);
#endif*/
    }

    public void StopAudio(string _name)
    {
        if (LOLSDK.Instance.IsInitialized)
            LOLSDK.Instance.StopSound(_name);
    }

    private void PlayEditorAudio(string _name, bool loop)
    {
        _name = System.IO.Path.GetFileNameWithoutExtension(_name);
        if (loop)
        {
            GameObject go = new GameObject();
            go.name = _name;
            AudioSource audioSrc = go.AddComponent<AudioSource>();
            audioSrc.clip = audioClips[_name];
            audioSrc.loop = true;
            audioSrc.Play();            
            Object.DontDestroyOnLoad(go);
        }
        else
        {
            if (!musicPlayer)
            {
                GameObject go = new GameObject();
                go.name = "musicPlayer";
                musicPlayer = go.AddComponent<AudioSource>();
            }
            try
            {
                musicPlayer.PlayOneShot(audioClips[_name]);
            }
            catch
            {
                Debug.Log("Could not find clip: " + _name);
            }
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
        }

        //_name = System.IO.Path.GetFileNameWithoutExtension(_name);
        //AudioClip ac = Resources.Load<AudioClip>("Music/" + _name);
        //musicPlayer.PlayOneShot(Resources.Load<AudioClip>("Music/" + _name));
    }

}