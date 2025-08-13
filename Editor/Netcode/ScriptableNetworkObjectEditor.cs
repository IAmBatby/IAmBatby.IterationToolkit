using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using IterationToolkit.Editor;
using IterationToolkit.Netcode;
using System;

[CustomEditor(typeof(ScriptableNetworkObject), true)]
[CanEditMultipleObjects]
public class ScriptableNetworkObjectEditor : Editor
{
    SerializedProperty byteArray;
    private SerializedProperty extendedGuid;
    private ScriptableNetworkObject networkObject;

    private void OnEnable()
    {
        networkObject = (ScriptableNetworkObject)target;
        extendedGuid = serializedObject.Seek("ExtendedGuid");
        byteArray = extendedGuid.Seek("m_Guid");
        
    }

    public override void OnInspectorGUI()
    {
        RefreshGuid();
        base.OnInspectorGUI();
    }

    private void RefreshGuid()
    {       
        if (networkObject.GuidIsUnique == false)
        {
            byte[] newGuid = Guid.NewGuid().ToByteArray();
            byteArray.ClearArray();
            for (int i = 0; i < newGuid.Length; i++)
            {
                byteArray.InsertArrayElementAtIndex(i);
                byteArray.GetArrayElementAtIndex(i).uintValue = newGuid[i];
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorUtility.SetDirty(target);
        }    
    }
}
