using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [AddComponentMenu("")]
    public class ParticlePlayer : MonoBehaviour
    {
        private Dictionary<ParticleSystem, ParticleSystem> assetToInstancedParticleDict = new Dictionary<ParticleSystem, ParticleSystem>();
        public static ParticlePlayer Create(MonoBehaviour target) => Create(target.gameObject);

        public static ParticlePlayer Create(GameObject target)
        {
            ParticlePlayer newPlayer = target.AddComponent<ParticlePlayer>();
            return (newPlayer);
        }

        public void PlayParticle(ParticlePreset preset)
        {
            if (preset == null || preset.Particle == null) return;
            ParticleSystem systemToPlay = null;
            if (assetToInstancedParticleDict.TryGetValue(preset.Particle, out ParticleSystem spawnedSystem))
                systemToPlay = spawnedSystem;
            else
            {
                ParticleSystem newInstance = GameObject.Instantiate(preset.Particle);
                newInstance.transform.position = transform.position;
                newInstance.transform.SetParent(transform, true);
                assetToInstancedParticleDict.Add(preset.Particle, newInstance);
                systemToPlay = newInstance;
            }

            if (systemToPlay != null)
            {
                ApplyParticlePresetSettings(systemToPlay, preset);
                systemToPlay.Play();
            }
        }

        public void ApplyParticlePresetSettings(ParticleSystem particleSystem, ParticlePreset preset)
        {
            if (particleSystem == null || preset == null || preset.UseParticleValues == false) return;
            ParticleSystem.MainModule module = particleSystem.main;
            module.startSize = new ParticleSystem.MinMaxCurve(preset.StartSize.x, preset.StartSize.y);
            module.startSpeed = new ParticleSystem.MinMaxCurve(preset.StartSpeed.x, preset.StartSpeed.y);
            module.startLifetime = new ParticleSystem.MinMaxCurve(preset.Lifetime, preset.Lifetime);
            module.duration = preset.Duration;
        }

        public void DetatchParticles()
        {
            foreach (KeyValuePair<ParticleSystem, ParticleSystem> kvp in assetToInstancedParticleDict)
                if (kvp.Value != null)
                    kvp.Value.transform.SetParent(null, true);
        }

        public void StopParticle(ParticlePreset preset)
        {
            if (preset == null || preset.Particle == null) return;
            if (assetToInstancedParticleDict.TryGetValue(preset.Particle, out ParticleSystem spawnedSystem))
                spawnedSystem.Stop();
        }
    }
}
