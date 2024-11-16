using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "IterationToolkit/AudioData", order = 1)]
    public class AudioPreset : ScriptableObject
    {
        [Header("Audio Values")]
        public Vector2 audioRandomPitchMinMax;
        [Range(0, 1)]
        public float audioVolume;

        [Header("Contextual Values")]
        [Space(10)]
        public bool onlyPlayOnInactiveSource;
        public bool shouldLoop;

        [Space(10)]
        [Header("Audio Assets")]
        public List<AudioClip> audioRandomClipList = new List<AudioClip>();
        public AudioMixer audioMixer;
    }
}
