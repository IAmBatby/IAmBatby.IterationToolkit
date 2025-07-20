using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable] //For debugging, this doesn't need to be serialized.
    public class MaterialCache //We may prefer this as a struct in the future. I don't wanna deal with that now though
    {
        public bool OverrideState => rendererInfos.Count > 0 ? rendererInfos[0].OverrideState : false;
        [SerializeField] private List<RendererInfo> rendererInfos = new List<RendererInfo>();

        private MonoBehaviour coroutineHostBehaviour;

        public MaterialCache(MonoBehaviour coroutineHost, List<Renderer> targets) => Initialize(coroutineHost, targets);

        public MaterialCache(MonoBehaviour parentTarget) => Initialize(parentTarget, parentTarget.GetComponentsInChildren<Renderer>());

        private void Initialize(MonoBehaviour coroutineHost, List<Renderer> targets)
        {
            coroutineHostBehaviour = coroutineHost;
            foreach (Renderer r in targets)
                if (r is not LineRenderer)
                    rendererInfos.Add(new RendererInfo(r, coroutineHostBehaviour));
        }

        private void Initialize(MonoBehaviour coroutineHost, Renderer[] targets)
        {
            coroutineHostBehaviour = coroutineHost;
            foreach (Renderer r in targets)
                if (r is not LineRenderer)
                    rendererInfos.Add(new RendererInfo(r, coroutineHostBehaviour));
        }

        public void Override(Material material)
        {
            foreach (RendererInfo r in rendererInfos)
                r.OverrideRenderer(material);
        }

        public void Override(Material material, float time)
        {
            foreach (RendererInfo r in rendererInfos)
                r.OverrideRenderer(material, time);
        }

        public void Override(VisualPreset preset, bool revertIfOverriden = false)
        {
            if (preset == null) return;
            if (preset.OnlyOverrideIfReverted && OverrideState) return;
            foreach (RendererInfo r in rendererInfos)
            {
                if (revertIfOverriden == true && OverrideState == true)
                    r.RevertRenderer();
                else
                    r.OverrideRenderer(preset);
            }
        }

        public void ConditionalOverride(VisualPreset preset, bool value)
        {
            if (value)
                Override(preset);
            else
                Revert();
        }

        public void ConditionalOverride(ReactionInfo info, bool value) => ConditionalOverride(info.VisualPreset, value);
        public void Revert()
        {
            foreach (RendererInfo r in rendererInfos)
                r.RevertRenderer();
        }
    }

    [System.Serializable]
    public struct RendererInfo
    {
        [field: SerializeField] public Renderer Renderer { get; private set; }
        [field: SerializeField] public List<Material> OriginalMaterials { get; private set; }
        [field: SerializeField] public bool OverrideState { get; private set; }

        private MonoBehaviour coroutineHost;

        [SerializeField] private List<Material> overrideMaterials;
        [SerializeField] private Timer overrideTimer;

        public RendererInfo(Renderer renderer, MonoBehaviour coroutineHostBehaviour)
        {
            coroutineHost = coroutineHostBehaviour;
            Renderer = renderer;
            OriginalMaterials = new List<Material>(renderer.sharedMaterials);
            OverrideState = false;
            overrideMaterials = new List<Material>(OriginalMaterials);
            overrideTimer = new Timer();
            overrideTimer.OnTimerFinish.AddListener(RevertRenderer);
        }

        public void OverrideRenderer(VisualPreset preset)
        {
            if (Mathf.Approximately(0f, preset.OverrideLength))
                OverrideRenderer(preset.OverrideMaterial);
            else
                OverrideRenderer(preset.OverrideMaterial, preset.OverrideLength);
        }

        public void OverrideRenderer(Material material, float timeUntilRevert)
        {
            overrideTimer.TryStopTimer();
            OverrideRenderer(material);
            overrideTimer.StartTimer(coroutineHost, timeUntilRevert);
        }

        public void OverrideRenderer(Material material)
        {
            for (int i = 0; i < overrideMaterials.Count; i++)
                overrideMaterials[i] = material;

            Renderer.SetMaterials(overrideMaterials);
            OverrideState = true;
        }

        public void RevertRenderer()
        {
            Renderer.SetMaterials(OriginalMaterials);
            OverrideState = false;
            overrideTimer.TryStopTimer();
        }
    }
}
