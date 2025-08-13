using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace IterationToolkit.Editor
{
    public static class EditorExtensions
    {
        public static SerializedProperty Seek(this SerializedProperty property, string propertyName) => EditorUtilities.Seek(property, propertyName);
        public static SerializedProperty Seek(this SerializedObject serializedObject, string propertyName) => EditorUtilities.Seek(serializedObject, propertyName);
    }
}
