using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IterationToolkit
{
    public static class CheatManager
    {
        //private static List<CheatEntry> registeredCheats = new List<CheatEntry>();
        private static Dictionary<string, List<CheatEntry>> registeredCheats = new Dictionary<string, List<CheatEntry>>();
        private static SelectableCollection<string> cheatCategories = new SelectableCollection<string>();
        private static Vector2 cheatWindowSizeScale = new Vector2(4.25f, 2f);
        private static Vector2 consoleScrollView;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            registeredCheats.Clear();
            cheatCategories = null;
        }

        /*
        public static void RegisterCheat<T>(ParameterEvent<T> action, T value, Rect rect)
        {
            bool useCheat = false;
            GUIUtilities.SetRenderButton(ref useCheat, GUIUtilities.GetName(value), rect);
            if (useCheat)
                action.Invoke(value);
        }*/

        public static void RenderCheats()
        {
            if (cheatCategories == null) return;
            float smallest = Mathf.Min(Screen.width, Screen.height);
            float consoleWidth = smallest / cheatWindowSizeScale.x;
            float consoleHeight = smallest / cheatWindowSizeScale.y;
            Rect rect = new Rect(Screen.width - (consoleWidth + (consoleWidth / 12.5f)), Screen.height - (consoleHeight + (consoleHeight / 9f)), consoleWidth, consoleHeight);

            GUILayout.BeginArea(rect, GUIDefaults.UI_Background);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label(cheatCategories.ActiveSelection.ToBold().Colorize(Color.white), GUIDefaults.UI_Header);
            GUILayout.Space(10);

            consoleScrollView = GUILayout.BeginScrollView(consoleScrollView, false, alwaysShowVertical: true);
            List<CheatEntry> activeCheats = registeredCheats[cheatCategories.ActiveSelection];
            for (int i = 0; i < activeCheats.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(activeCheats[i].GetCheatName().ToBold(), GUILayout.Width(50));
                RenderCheat(activeCheats[i]);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUILayout.FlexibleSpace();
        }

        public static void RenderCheat(CheatEntry cheat)
        {
            bool shouldCheat = false;
            shouldCheat = GUIUtilities.RenderButton(shouldCheat, string.Empty);
            if (shouldCheat)
                cheat.Cheat();
        }

        public static void RegisterCheat(CheatEntry cheat, string category = "General")
        {
            AddCheat(cheat, category);
        }

        public static void RegisterCheat(Action action, string category = "General")
        {
            CheatEntry cheat = new CheatEntry();
            cheat.SetCheat(action);
            AddCheat(cheat, category);
        }

        public static void RegisterCheat<T>(ParameterEvent<T> newEvent, T newValue, string category = "General")
        {
            CheatEntry<T> cheat = new CheatEntry<T>();
            cheat.SetCheat(newEvent, newValue);
            AddCheat(cheat, category);
        }

        private static void AddCheat(CheatEntry cheat, string category)
        {
            if (registeredCheats.TryGetValue(category, out List<CheatEntry> cheats))
                cheats.Add(cheat);
            else
            {
                registeredCheats.Add(category, new List<CheatEntry> { cheat });
                if (cheatCategories == null)
                    cheatCategories = new SelectableCollection<string>(registeredCheats.Keys.ToList());
                else
                    cheatCategories.AddObject(category);
            }
        }

        public static void ToggleCheatCategoryForward() => cheatCategories?.SelectForward();
        public static void ToggleCheatCategoryBackward() => cheatCategories?.SelectBackward();

           
    }

    public class CheatEntry
    {
        protected Action cheatEvent;

        public void SetCheat(Action newCheatEvent) => cheatEvent = newCheatEvent;

        public virtual void Cheat() => cheatEvent?.Invoke();

        public virtual string GetCheatName() => string.Empty;
    }

    public class CheatEntry<T> : CheatEntry
    {
        private T value;
        private ParameterEvent<T> valueCheatEvent;

        public void SetCheat(ParameterEvent<T> newCheatEvent, T newValue)
        {
            value = newValue;
            valueCheatEvent = newCheatEvent;
        }

        public override void Cheat() => valueCheatEvent?.Invoke(value);
        public override string GetCheatName() => GUIUtilities.GetName(value);
    }
}
