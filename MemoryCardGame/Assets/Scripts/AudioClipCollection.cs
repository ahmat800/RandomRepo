using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioCollection", menuName = "AudioCollections/CreateAudioCollection")]
public class AudioClipCollection : ScriptableObject
{
    public List<AudioUnit> AudioUnits;



    public AudioClip GetAudioClip(string clipName)
    {
        for (int i = 0; i < AudioUnits.Count; i++)
        {
            if (AudioUnits[i].name == clipName)
                return AudioUnits[i].clip;
        }
        return null;
    }


}


[Serializable]
public class AudioUnit
{
    public string name;
    public AudioClip clip;
}

