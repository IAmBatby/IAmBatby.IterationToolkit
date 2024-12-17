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
    }
}
