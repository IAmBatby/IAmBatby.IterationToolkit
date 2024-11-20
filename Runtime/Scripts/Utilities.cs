using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit
{
    public static class Utilities
    {
        public static int IncreaseIndex(int index, int count) => index != count ? index + 1 : 0;

        public static int IncreaseIndex<T>(int index, List<T> collection) => index != collection.Count - 1 ? index + 1 : 0;

        public static int DecreaseIndex(int index, int count) => index != 0 ? index - 1 : count;

        public static int DecreaseIndex<T>(int index, List<T> collection) => index != 0 ? index - 1 : collection.Count - 1;

        public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;
        public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;

        public static void OrganizeObjects<T>(GameObject parent, List<T> objects, bool renameObjects) where T: MonoBehaviour
        {
            Type genericType = typeof(T);
            GameObject childParentFolder = new GameObject(genericType.Name + " Collection");
            childParentFolder.transform.parent = parent.transform;

            for (int i = 0; i < objects.Count; i++)
            {
                GameObject obj = objects[i].gameObject;
                if (obj.transform.parent == parent)
                {
                    obj.transform.parent = childParentFolder.transform;
                    if (renameObjects)
                        obj.name = genericType.Name + "#" + i;
                }
            }
        }

        public static void DrawPrefabPreview(Transform context, GameObject prefab, Color primary, Color secondary, ref List<MeshFilter> filters)
        {
            GizmosCache cache = new GizmosCache(context);

            if (prefab == null) return;
            if (filters == null)
                filters = new List<MeshFilter>();
            if (filters.Count == 0)
                foreach (MeshFilter renderer in prefab.GetComponentsInChildren<MeshFilter>())
                    filters.Add(renderer);
            
            for (int i = 0;i < filters.Count;i++)
                if (filters[i].transform.root != prefab.transform)
                {
                    filters.Clear();
                    return;
                }

            Gizmos.color = new Color(primary.r, primary.g, primary.b, 0.3f);
            foreach (MeshFilter renderer in filters)
                Gizmos.DrawMesh(renderer.sharedMesh, renderer.transform.position, renderer.transform.rotation, renderer.transform.lossyScale);
            Gizmos.color = new Color(secondary.r, secondary.g, secondary.b, 0.05f);
            foreach (MeshFilter renderer in filters)
                Gizmos.DrawWireMesh(renderer.sharedMesh, renderer.transform.position, renderer.transform.rotation, renderer.transform.lossyScale);

            cache.Revert();
        }

        public static void DrawSnapToGroundPreview(Transform transform, int layerMask = ~0, float yOffset = 0f)
        {
            GizmosCache cache = new GizmosCache(transform);
            Vector3 boxSize = new Vector3(0.3f, 0.3f, 0.3f);

            if (TryGetSnap(transform, layerMask, out Vector3 snapPos, yOffset, Mathf.Infinity))
            {
                DrawLine(Vector3.zero, snapPos - transform.position, Color.white);
                DrawWireCube(Vector3.zero, boxSize, Color.yellow);
                DrawWireCube(snapPos - transform.position, boxSize, Color.green);
            }
            else
                DrawWireCube(Vector3.zero, boxSize, Color.red);

            cache.Revert();
        }

        public static bool TryGetSnap(Transform transform, LayerMask layerMask, out Vector3 snapPosition, float yOffset = 0f, float distance = Mathf.Infinity)
        {
            snapPosition = Vector3.negativeInfinity;

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, distance, layerMask))
                snapPosition = hit.point + new Vector3(0, yOffset, 0);

            return (snapPosition != Vector3.negativeInfinity);
        }

        public static void DrawWireCube(Vector3 position, Vector3 scale, Color color)
        {
            Color previousColor = SwitchGizmosColors(color);
            Gizmos.DrawWireCube(position, scale);
            Gizmos.color = previousColor;
        }

        public static void DrawLine(Vector3 to, Vector3 from, Color color)
        {
            Color previousColor = SwitchGizmosColors(color);
            Gizmos.DrawLine(to, from);
            Gizmos.color = previousColor;
        }

        public static Color SwitchGizmosColors(Color newColor)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = newColor;
            return (oldColor);
        }
    }
}

public struct GizmosCache
{
    private Matrix4x4 matrix;
    private Color color;
    private Transform transform;

    public GizmosCache(Transform newTransform = null)
    {
        matrix = Gizmos.matrix;
        color = Gizmos.color;
        transform = newTransform;

        if (transform != null)
            Gizmos.matrix = transform.localToWorldMatrix;
    }

    public void Revert()
    {
        Gizmos.matrix = matrix;
        Gizmos.color = color;
    }
}
