using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [AddComponentMenu("")]
    public class ReactionPlayer : MonoBehaviour
    {
        public AudioPlayer Audio { get; private set; }
        public ParticlePlayer Particles { get; private set; }

        public static ReactionPlayer Create(MonoBehaviour target) => Create(target.gameObject);

        public static ReactionPlayer Create(GameObject target)
        {
            ReactionPlayer newPlayer = target.AddComponent<ReactionPlayer>();
            newPlayer.Audio = AudioPlayer.Create(newPlayer);
            newPlayer.Particles = ParticlePlayer.Create(newPlayer);
            return (newPlayer);
        }

        public void Play(ReactionInfo info)
        {
            Audio.PlayAudio(info.AudioPreset);
            Particles.PlayParticle(info.ParticlePreset);
        }

        public void Play(AudioPreset audioPreset)
        {
            Audio.PlayAudio(audioPreset);
        }

        public void Play(ParticlePreset particlePreset)
        {
            Particles.PlayParticle(particlePreset);
        }
    }
}
