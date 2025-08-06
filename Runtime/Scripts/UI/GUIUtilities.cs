using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IterationToolkit
{
    public static class GUIUtilities
    {
        private static Rect previousRect;
        private static float padding = 5;
        private static bool inSpace;

        public enum LayoutDisplayMode { Horizontal, Vertical };
        public static LayoutDisplayMode DisplayMode { get; set; } = LayoutDisplayMode.Horizontal;

        public static void RenderScriptableSettings(List<ScriptableSetting> settings, Rect rect)
        {
            for (int i = 0; i < settings.Count; i++)
                RenderScriptableSetting(settings[i], new Rect(rect.x, rect.y + (rect.height * i), rect.width, rect.height));
        }
        public static void RenderScriptableSetting(ScriptableSetting setting, Rect rect)
        {
            OpenSpace(rect);
            if (setting is ScriptableAudioSetting audioSetting)
                audioSetting.Volume.Value = RenderSliderValue(audioSetting.Volume.Value, audioSetting.DbMinMax.Value.x, audioSetting.DbMinMax.Value.y, audioSetting.name);
            CloseSpace();
        }

        public static T RenderValue<T>(T value, string header, Rect rect)
        {
            T returnValue = default;
            OpenSpace(rect);
            if (value is bool boolValue && RenderButton(boolValue, header) is T tValue)
                returnValue = tValue;
            CloseSpace();
            return (returnValue);
        }

        public static void OpenSpace(Rect rect)
        {
            if (inSpace == false)
            {
                GUILayout.BeginArea(rect, GUIDefaults.UI_Background);
                inSpace = true;
            }
            GUILayout.Space(padding);
            OpenDirection();
            GUILayout.Space(padding);
        }

        public static void CloseSpace()
        {
            GUILayout.Space(padding);
            CloseDirection();
            GUILayout.Space(padding);
            if (inSpace == true)
            {
                GUILayout.EndArea();
                inSpace = false;
            }
        }

        public static void OpenDirection()
        {
            if (DisplayMode == LayoutDisplayMode.Horizontal)
                GUILayout.BeginHorizontal();
            else
                GUILayout.BeginVertical();
        }

        public static void CloseDirection()
        {
            if (DisplayMode == LayoutDisplayMode.Horizontal)
                GUILayout.EndHorizontal();
            else
                GUILayout.EndVertical();
        }

        public static int RenderOptions(int index, string[] values, int xCount, Rect rect)
        {
            int returnIndex = index;
            OpenSpace(rect);
            returnIndex = GUILayout.SelectionGrid(returnIndex, values, xCount);
            CloseSpace();
            return (returnIndex);
        }

        public static void SetRenderOption<T>(T value, List<T> values, Rect rect) where T : UnityEngine.Object
        {
            int index = values.Contains(value) ? values.IndexOf(value) : 0;
            value = RenderOptions(index, values, rect);
        }

        public static void SetRenderOption<T>(ref T value, List<T> values, Rect rect)
        {
            int index = values.Contains(value) ? values.IndexOf(value) : 0;
            value = RenderOptions(index, values, rect);
        }

        public static T RenderOptions<T>(int currentIndex, List<T> values, Rect rect)
        {
            int xCount = DisplayMode == LayoutDisplayMode.Horizontal ? values.Count : 1;
            string[] valueNames = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
                valueNames[i] = GetName(values[i]);
            return (values[RenderOptions(currentIndex, valueNames, xCount, rect)]);
        }

        public static T RenderOptions<T>(int currentIndex, T[] values, Rect rect)
        {
            int xCount = DisplayMode == LayoutDisplayMode.Horizontal ? values.Length : 1;
            string[] valueNames = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                valueNames[i] = GetName(values[i]);
            return (values[RenderOptions(currentIndex, valueNames, xCount, rect)]);
        }

        public static T RenderOptions<T>(int currentIndex, T[] values, string[] valueNames, Rect rect)
        {
            return (values[RenderOptions(currentIndex, valueNames, values.Length, rect)]);
        }

        public static void SetRenderButton(ref bool value, string header, Rect rect)
        {
            OpenSpace(rect);
            value = RenderButton(header);
            CloseSpace();
        }

        public static bool RenderButton(bool value, string header, bool useHeader = false)
        {
            if (useHeader)
            {
                TryRenderHeader(header);
                return (GUILayout.Toggle(value, GUIContent.none));
            }
            else
                return (GUILayout.Toggle(value, header));
        }

        public static bool RenderButton(string header)
        {
            //TryRenderHeader(header);
            return (GUILayout.Button(header));
        }

        public static float RenderSliderValue(float value, float min, float max, string header)
        {
            TryRenderHeader(header);
            return (GUILayout.HorizontalSlider(value, min, max));
        }

        public static float RenderSliderValue(float value, Vector2 minMax, string Header)
        {
            return (RenderSliderValue(value, minMax.x, minMax.y, Header));
        }

        public static void TryRenderHeader(string header)
        {
            if (string.IsNullOrEmpty(header)) return;
            GUILayout.Label(header.ToBold().Colorize(Color.white), GUIDefaults.UI_Header);
        }

        public static string GetName<T>(T value)
        {
            if (value == null) return string.Empty;
            if (value is UnityEngine.Object obj)
                return (obj.name);
            else
                return (value.ToString());
        }

        public static Rect WorldPointToSizedRect(Vector3 position, GUIContent content, GUIStyle style)
        {
            if (style == null) return (new Rect(0, 0, 0, 0));
            Vector2 vector = WorldToGUIPointWithDepth(position);
            Vector2 vector2 = style.CalcSize(content);
            Rect rect = new Rect(vector.x, vector.y, vector2.x, vector2.y);
            switch (style.alignment)
            {
                case TextAnchor.UpperCenter:
                    rect.x -= rect.width * 0.5f;
                    break;
                case TextAnchor.UpperRight:
                    rect.x -= rect.width;
                    break;
                case TextAnchor.MiddleLeft:
                    rect.y -= rect.height * 0.5f;
                    break;
                case TextAnchor.MiddleCenter:
                    rect.x -= rect.width * 0.5f;
                    rect.y -= rect.height * 0.5f;
                    break;
                case TextAnchor.MiddleRight:
                    rect.x -= rect.width;
                    rect.y -= rect.height * 0.5f;
                    break;
                case TextAnchor.LowerLeft:
                    rect.y -= rect.height;
                    break;
                case TextAnchor.LowerCenter:
                    rect.x -= rect.width * 0.5f;
                    rect.y -= rect.height;
                    break;
                case TextAnchor.LowerRight:
                    rect.x -= rect.width;
                    rect.y -= rect.height;
                    break;
            }

            return style.padding.Add(rect);
        }

        public static Vector2 WorldToGUIPointWithDepth(Vector3 world) => WorldToGUIPointWithDepth(Camera.main, world);
        public static Vector2 WorldToGUIPointWithDepth(Camera camera, Vector3 world)
        {
            world = Gizmos.matrix.MultiplyPoint(world);
            if ((bool)camera)
            {
                Vector3 vector = camera.WorldToScreenPoint(world);
                vector.y = (float)camera.pixelHeight - vector.y;
                Vector2 vector2 = PixelsToPoints(vector);
                return new Vector3(vector2.x, vector2.y, vector.z);
            }

            return world;
        }

        private static MethodInfo pixelsPerPointGetter;
        public static float PixelsPerPoint
        {
            get
            {
                if (pixelsPerPointGetter == null)
                    pixelsPerPointGetter = typeof(GUIUtility).GetMethod("get_pixelsPerPoint", BindingFlags.Static | BindingFlags.NonPublic);
                return ((float)pixelsPerPointGetter.Invoke(null, null));
            }
        }

        public static Vector2 PixelsToPoints(Vector2 position)
        {
            //float num = 1f / PixelsPerPoint;
            float num = 1f / 1;
            position.x *= num;
            position.y *= num;
            return position;
        }

        public static void RenderBoxedString(Vector3 position, string text, GUIStyle style)
        {
            Color old = GUI.color;
            GUI.color = Color.red;
            GUIContent testLabel = new GUIContent(text, Texture2D.blackTexture);
            Rect test = GUIUtilities.WorldPointToSizedRect(position, testLabel, style);
            GUIStyle newStyle = new GUIStyle(style);
            GUI.Box(test, testLabel, newStyle);
            GUI.color = old;

        }

        private static Texture2D invertedBox;
        public static Texture2D InvertedBox
        {
            get
            {
                if (invertedBox == null)
                    invertedBox = InvertTexture(GUI.skin.box.normal.background);
                return (invertedBox);
            }
        }

        public static Texture2D InvertTexture(Texture2D texture)
        {
            Texture2D newText = new Texture2D(texture.width, texture.height);
            Color[] old = texture.GetPixels();
            for (int i = 0; i < old.Length; i++)
                old[i] = new Color(1 - old[i].r,1 - old[i].g,1 - old[i].b, 1 - old[i].a);
            newText.SetPixels(old);
            newText.Apply();
            return (newText);
        }

        public static List<Rect> SplitRects(Rect target, int splitCount)
        {
            List<Rect> rects = new List<Rect>();
            for (int i = 0; i < splitCount; i++)
                rects.Add(new Rect(target.x + (i * (target.width / splitCount)), target.y, target.width / splitCount, target.height));
            return (rects);
        }
    }
}
