using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.Editor
{
    public static class EditorUtilities
    {
        public static bool InEditor => IsInEditorCheck();

        static bool IsInEditorCheck()
        {
            bool returnBool = false;
#if (UNITY_EDITOR)
            returnBool = true;
#endif
            return (returnBool);
        }

        public static IEnumerable<Type> GetTypes(Type filterType)
        {
            List<Type> moveTypes = new List<Type>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                //Debug.Log(type.ToString());
                if (type == filterType || type.IsSubclassOf(filterType))
                    moveTypes.Add(type);
            }

            return (moveTypes);
        }

        public static List<T> FindAssets<T>(params string[] directories) where T : UnityEngine.Object
        {
            List<T> returnList = new List<T>();
            foreach (string guid in AssetDatabase.FindAssets("t:" + typeof(T).Name, directories))
                returnList.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
            return (returnList);
        }

        public static IEnumerable<ScriptableObject> GetScriptableObjects(Type type)
        {
            if (InEditor)
            {
                IEnumerable<ScriptableObject> allScriptableObjects;
                List<ScriptableObject> returnScriptableObjects = new List<ScriptableObject>();


                allScriptableObjects = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(x));

                foreach (ScriptableObject item in allScriptableObjects)
                {
                    if (item.GetType() == type)
                        returnScriptableObjects.Add(item);
                    else if (item.GetType().BaseType != null && item.GetType().BaseType == type)
                        returnScriptableObjects.Add(item);
                }
                return (returnScriptableObjects);
            }
            else
                return (null);
        }

        public static IEnumerable<T> GetScriptableObjects<T>() where T : UnityEngine.Object
        {
            if (!InEditor) return (null);

            List<T> returnScriptableObjects = new List<T>();

            IEnumerable<ScriptableObject> allScriptableObjects = AssetDatabase.FindAssets("t:ScriptableObject")
            .Select(x => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(x)));

            foreach (ScriptableObject item in allScriptableObjects)
                if (item is T castedItem)
                    returnScriptableObjects.Add(castedItem);

            return (returnScriptableObjects);
        }

        public static string Decorate(string name) => "<" + name + ">k__BackingField";

        public static SerializedProperty Seek(SerializedObject target, string targetField) => Seek(target.GetIterator(), targetField);

        public static SerializedProperty Seek(SerializedProperty target, string targetField)
        {
            foreach (SerializedProperty sp in EditorLabelUtilities.FindSerializedProperties(target.Copy()))
                if (sp.name.Contains(targetField) || sp.name.Contains(Decorate(targetField)))
                    return (sp.Copy());
            return (null);
        }

        public static void DrawProperties(SerializedProperty source, Rect totalSize, params string[] propertyNames)
        {
            List<Rect> rects = GUIUtilities.SplitRects(totalSize, propertyNames.Length);
            for (int i = 0; i < rects.Count; i++)
                EditorGUI.PropertyField(rects[i], source.Seek(propertyNames[i]), GUIContent.none);
        }
    }
}
