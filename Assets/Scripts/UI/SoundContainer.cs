using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Volumes", order = 1)]
public class SoundContainer : ScriptableObject
{
  public List<AudioSound> audioSounds;

  void Start()
  {
    for (int i = 0; i < audioSounds.Count; i++)
    {
      audioSounds[i].volume = 1;
    }
  }
}
