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
                obj.transform.parent = childParentFolder.transform;
                if (renameObjects)
                    obj.name = genericType.Name + "#" + i;
            }
        }
    }
}
