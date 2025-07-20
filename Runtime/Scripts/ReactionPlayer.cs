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
        public MaterialCache MaterialCache { get; private set; }

        public static ReactionPlayer Create(MonoBehaviour target)
        {
            ReactionPlayer newPlayer = target.gameObject.AddComponent<ReactionPlayer>();
            newPlayer.Audio = AudioPlayer.Create(newPlayer);
            newPlayer.Particles = ParticlePlayer.Create(newPlayer);
            newPlayer.MaterialCache = new MaterialCache(target);
            return (newPlayer);
        }

        public void Play(ReactionInfo info)
        {
            Audio.PlayAudio(info.AudioPreset);
            Particles.PlayParticle(info.ParticlePreset);
            MaterialCache.Override(info.VisualPreset);
        }

        public void Play(AudioPreset audioPreset) => Audio.PlayAudio(audioPreset);
        public void Play(ParticlePreset particlePreset) => Particles.PlayParticle(particlePreset);
        public void Play(VisualPreset visualPreset) => MaterialCache.Override(visualPreset);
        public void Play(VisualPreset visualPreset, bool conditional) => MaterialCache.ConditionalOverride(visualPreset, conditional);

        public void PlayAudio(ReactionInfo info) => Play(info.AudioPreset);
        public void PlayParticle(ReactionInfo info) => Play(info.ParticlePreset);
        public void PlayVisual(ReactionInfo info) => Play(info.VisualPreset);
        public void PlayVisual(ReactionInfo info, bool conditional) => Play(info.VisualPreset, conditional);
    }
}
