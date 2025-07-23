using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public static class GUIDefaults
    {
        public static GUIStyle UI_Background { get; private set; }
        public static GUIStyle UI_Header { get; private set; }
        public static GUIStyle UI_Label { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeDefaults()
        {
            UI_Background = LabelUtilities.CreateStyle(enableRichText: true, Color.black.SetAlpha(0.45f));
            UI_Header = LabelUtilities.CreateStyle(true);
            UI_Header.fontSize = 18;
            UI_Label = LabelUtilities.CreateStyle(true);

        }
    }
}
