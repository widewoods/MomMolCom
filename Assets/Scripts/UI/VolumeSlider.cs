using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
  [SerializeField] private Slider slider;
  [SerializeField] private SoundContainer soundContainer;
  [SerializeField] private int index;

  void Awake()
  {
    if (slider == null) slider = GetComponent<Slider>();
    slider.value = soundContainer.audioSounds[index].volume;
  }

  public void OnSliderValueChanged()
  {
    soundContainer.audioSounds[index].volume = slider.value;
  }
}
