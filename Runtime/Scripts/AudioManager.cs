using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : GlobalManager
    {
        private static AudioManager _instance;
        public static new AudioManager Instance => SingletonManager.GetSingleton<AudioManager>(typeof(AudioManager));

        private static AudioSource _audioSource;
        private static AudioSource ManagerSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = Instance.GetComponent<AudioSource>();
                return _audioSource;
            }
        }


        public static void PlayAudio(AudioPreset data, AudioSource source, AudioClip overrideClip = null)
        {
            if (!ShouldPlay(data, source)) return;

            ApplyAudioData(data, source);
            if (overrideClip != null)
                source.clip = overrideClip;
            source.Play();
        }

        public static void PlayAudio(AudioPreset data, Vector3 position)
        {
            ManagerSource.transform.position = position;
            PlayAudio(data, ManagerSource);
        }

        public static void ApplyAudioData(AudioPreset data, AudioSource source)
        {
            source.outputAudioMixerGroup  = data.audioMixer.outputAudioMixerGroup;
            source.volume = data.audioVolume;
            source.pitch = Random.Range(data.audioRandomPitchMinMax.x, data.audioRandomPitchMinMax.y);
            source.clip = data.audioRandomClipList[Random.Range(0, data.audioRandomClipList.Count)];
            source.loop = data.shouldLoop;
        }

        private static bool ShouldPlay(AudioPreset data, AudioSource audioSource)
        {
            if (audioSource.isPlaying && data.onlyPlayOnInactiveSource)
                return (false);
            else
                return (true);
        }
    }
}
