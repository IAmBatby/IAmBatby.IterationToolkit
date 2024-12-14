using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [AddComponentMenu("")]
    public class AudioPlayer : MonoBehaviour
    {
        private Dictionary<int, AudioSource> audioSourceDict = new Dictionary<int, AudioSource>();

        public static AudioPlayer Create(MonoBehaviour target) => Create(target.gameObject);

        public static AudioPlayer Create(GameObject target)
        {
            AudioPlayer newPlayer = target.AddComponent<AudioPlayer>();
            return (newPlayer);
        }

        public void PlayAudio(AudioPreset audioPreset)
        {
            if (audioPreset == null) return;
            if (!audioSourceDict.ContainsKey(audioPreset.SoundPriority))
                GenerateNewAudioSource(audioPreset.SoundPriority);

            if (audioSourceDict.TryGetValue(audioPreset.SoundPriority, out AudioSource registeredSource))
                AudioManager.PlayAudio(audioPreset, registeredSource);
            else
                Debug.LogError("Failed To Find Registered AudioSource!");
        }

        public void RegisterAudioSource(int soundPriority, AudioSource audioSource)
        {
            if (audioSource == null) return;
            if (audioSourceDict.ContainsKey(soundPriority))
                audioSourceDict[soundPriority] = audioSource;
            else
                audioSourceDict.Add(soundPriority, audioSource);
        }

        private void GenerateNewAudioSource(int soundPriorityID)
        {
            if (audioSourceDict.ContainsKey(soundPriorityID))
            {
                Debug.LogError("AudioPlayer tried to register new AudioSource id that is already registered!");
                return;
            }
            GameObject newAudioSourceChild = new GameObject("AudioPlayerChildSource#" + soundPriorityID);
            AudioSource newAudioSource = newAudioSourceChild.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            newAudioSourceChild.transform.SetParent(transform);
            audioSourceDict.Add(soundPriorityID, newAudioSource);
        }
    }
}
