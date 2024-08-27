using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "IterationToolkit/AudioData", order = 1)]
    public class AudioPreset : ScriptableObject
    {
        public AudioMixer audioMixer;
        [Space(10)]
        public Vector2 audioRandomPitchMinMax;
        [Range(0, 1)]
        public float audioVolume;

        [Space(10)]
        public List<AudioClip> audioRandomClipList = new List<AudioClip>();
    }
}
