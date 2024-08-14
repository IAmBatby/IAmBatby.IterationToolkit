using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class SelectableCollection<T>
    {
        private bool isInitalized;

        [SerializeField] public List<T> allObjects;
        //public List<T> AllObjects => allObjects;
        private List<T> unselectedObjects;
        public T ActiveSelection
        {
            get
            {
                if (allObjects != null && allObjects.Count != 0 && selectionIndex <= allObjects.Count - 1)
                    return (allObjects[selectionIndex]);
                else
                    return (default);
            }
        }

        private int selectionIndex;

        public ExtendedEvent<T> onSelected = new ExtendedEvent<T>();
        public ExtendedEvent<T> onUnselected = new ExtendedEvent<T>();

        private Dictionary<T, List<Action>> onSelectedActionsDict;
        private List<Action> onSelectedActionsList;
        private Dictionary<T, List<Action>> onUnselectedActionsDict;

        public SelectableCollection(List<T> objects = null)
        {
            isInitalized = false;
            Initalize(objects);
        }

        public void Initalize(List<T> objects = null)
        {
            //allObjects = new List<T>(objects);
            //unselectedObjects = new List<T>(objects);
            allObjects = new List<T>();
            unselectedObjects = new List<T>();
            onSelectedActionsDict = new Dictionary<T, List<Action>>();
            onUnselectedActionsDict = new Dictionary<T, List<Action>>();
            onSelectedActionsList = new List<Action>();
            onSelected = new ExtendedEvent<T>();
            onUnselected = new ExtendedEvent<T>();

            if (objects != null)
                foreach (T newObject in objects)
                    AddObject(newObject);

            isInitalized = true;
        }

        public void AddObject(T newObject, bool selectOnAdd = false)
        {
            if (!allObjects.Contains(newObject))
            {
                allObjects.Add(newObject);
                unselectedObjects.Add(newObject);
                onSelectedActionsDict.Add(newObject, new List<Action>());
                onUnselectedActionsDict.Add(newObject, new List<Action>());
            }
            else
                Debug.LogError("Cannot add object to SelectableCollection as it has already been added.");
        }

        public void RemoveObject(T removalObject)
        {
            if (allObjects.Contains(removalObject))
            {
                //Debug.Log("Removing Item, Index Is At: " + selectionIndex);
                T activeSelection = default;
                bool selectPreviousActiveObject = false;
                if (allObjects.IndexOf(removalObject) != selectionIndex)
                {
                    activeSelection = ActiveSelection;
                    selectPreviousActiveObject = true;
                }
                allObjects.Remove(removalObject);
                unselectedObjects.Remove(removalObject);
                if (onSelectedActionsDict.ContainsKey(removalObject))
                    onSelectedActionsDict.Remove(removalObject);
                if (onUnselectedActionsDict.ContainsKey(removalObject))
                    onUnselectedActionsDict.Remove(removalObject);
                if (selectPreviousActiveObject == true)
                    Select(activeSelection);
                else
                    SelectBackward();
            }
        }

        public void AssignOnSelected(Action newSelectedAction)
        {
            TryInitalize();

            onSelectedActionsList.Add(newSelectedAction);
        }

        public void AssignOnSelected(IEnumerable<Action> newSelectedActions)
        {
            TryInitalize();

            foreach (Action action in newSelectedActions)
            {
                T convertedTarget = (T)action.Target;
                if (allObjects.Contains(convertedTarget))
                    if (!onSelectedActionsDict[convertedTarget].Contains(action))
                        onSelectedActionsDict[convertedTarget].Add(action);
            }
        }

        public void AssignOnUnselected(IEnumerable<Action> newUnselectedActions)
        {
            foreach (Action action in newUnselectedActions)
            {
                T convertedTarget = (T)action.Target;
                if (allObjects.Contains(convertedTarget))
                    if (!onUnselectedActionsDict[convertedTarget].Contains(action))
                        onUnselectedActionsDict[convertedTarget].Add(action);
            }
        }

        private void InvokeSelection(T selectedObject)
        {
            onSelected.Invoke(selectedObject);
            foreach (Action action in onSelectedActionsDict[selectedObject])
                action.Invoke();
            foreach (Action action in onSelectedActionsList)
                action.Invoke();
        }

        private void InvokeUnselection(T unselectedObject)
        {
            onUnselected.Invoke(unselectedObject);
            foreach (Action aciton in onUnselectedActionsDict[unselectedObject])
                aciton.Invoke();
        }

        public void Select(int index)
        {
            if (allObjects.Count != 0)
                Select(allObjects[index]);
        }

        public void Select(T gameObject)
        {
            TryInitalize();

            if (allObjects.Contains(gameObject))
            {
                //Debug.Log("Selecting Item " + gameObject + ", Index Is At: " + selectionIndex);
                selectionIndex = allObjects.IndexOf(gameObject);

                if (!unselectedObjects.Contains(ActiveSelection))
                    unselectedObjects.Add(ActiveSelection);


                if (unselectedObjects.Contains(ActiveSelection))
                    unselectedObjects.Remove(ActiveSelection);

                InvokeSelection(ActiveSelection);

                foreach (T unselectedObject in unselectedObjects)
                    InvokeUnselection(unselectedObject);
            }
        }

        public void SelectForward()
        {
            TryInitalize();
            if (allObjects.Count != 0)
                Select(allObjects[selectionIndex.Increase(allObjects)]);
        }

        public void SelectBackward()
        {
            TryInitalize();
            if (allObjects.Count != 0)
                Select(allObjects[selectionIndex.Decrease(allObjects)]);
        }

        public void TryInitalize()
        {
            if (isInitalized == false)
                Initalize(allObjects);
        }
    }
}
