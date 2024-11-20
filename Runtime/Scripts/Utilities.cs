using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
            Matrix4x4 previousMatrix = Gizmos.matrix;
            Color previousColor = Gizmos.color;
            Gizmos.matrix = context.localToWorldMatrix;
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

            Gizmos.color = previousColor;
            Gizmos.matrix = previousMatrix;
        }

        public static void DrawSnapToGroundPreview(Transform transform, int layerMask = ~0, float yOffset = 0f)
        {
            Matrix4x4 previousMatrix = Gizmos.matrix;
            Color previousColor = Gizmos.color;
            Gizmos.matrix = transform.localToWorldMatrix;

            Vector3 target = Vector3.positiveInfinity;

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, layerMask))
                target = hit.point + new Vector3(0, yOffset, 0);

            Gizmos.DrawLine(Vector3.zero, target);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(target, new Vector3(0.2f, 0.2f, 0.2f));

            Gizmos.color = previousColor;
            Gizmos.matrix = previousMatrix;
        }

        public static void TrySnapToGround(Transform transform, LayerMask layerMask, float yOffset = 0f, float distance = Mathf.Infinity)
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, distance, layerMask))
                transform.position = hit.point + new Vector3(0, yOffset, 0);
        }
    }
}
