using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : GlobalManager
    {
        new public static AudioManager Instance => Singleton.GetInstance<AudioManager>(ref _manager);

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


        public static void PlayAudio(AudioPreset data, AudioSource source)
        {
            if (!ShouldPlay(data, source)) return;

            ApplyAudioData(data, source);
            source.Play();
        }

        public static void PlayAudio(AudioPreset data, Vector3 position)
        {
            ManagerSource.transform.position = position;
            PlayAudio(data, ManagerSource);
        }

        private static void ApplyAudioData(AudioPreset data, AudioSource source)
        {
            source.outputAudioMixerGroup  = data.audioMixer.outputAudioMixerGroup;
            source.volume = data.audioVolume;
            source.pitch = Random.Range(data.audioRandomPitchMinMax.x, data.audioRandomPitchMinMax.y);
            source.clip = data.audioRandomClipList[Random.Range(0, data.audioRandomClipList.Count)];
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
