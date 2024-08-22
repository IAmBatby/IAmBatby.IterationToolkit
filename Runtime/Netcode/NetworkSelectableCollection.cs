#if NETCODE_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkSelectableCollection<T> : NetworkVariable<int>
    {
        private bool isInitalized;

        public bool IsServer => GameNetworkManager.Instance.IsServer;

        public List<T> Collection => allObjects;
        [SerializeField] private List<T> allObjects;
        //public List<T> AllObjects => allObjects;
        private List<T> unselectedObjects;
        public T ActiveSelection
        {
            get
            {
                if (allObjects != null && allObjects.Count != 0 && SelectionIndex <= allObjects.Count - 1)
                    return (allObjects[SelectionIndex]);
                else
                    return (default);
            }
        }

        private int SelectionIndex { get { return Value; } set { Value = value; } }

        public ExtendedEvent<T> onSelected = new ExtendedEvent<T>();
        public ExtendedEvent<T> onUnselected = new ExtendedEvent<T>();
        public ExtendedEvent onSelectionChange = new ExtendedEvent();

        private Dictionary<T, List<Action>> onSelectedActionsDict;
        private List<Action> onSelectedActionsList;
        private Dictionary<T, List<Action>> onUnselectedActionsDict;

        public NetworkSelectableCollection(int value = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(value, readPerm, writePerm)
        {
            isInitalized = false;
            Initalize();
            OnValueChanged += OnCollectionValueChanged;
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

        private void OnCollectionValueChanged(int previousValue, int newValue)
        {
            Debug.Log("Value Changed From: " + allObjects[previousValue] + "(" + previousValue + ")" + " To: " + allObjects[newValue] + "(" + newValue + ")");
            Debug.Log("Active Selection Is: " + ActiveSelection);

            if (!unselectedObjects.Contains(ActiveSelection))
                unselectedObjects.Add(ActiveSelection);


            if (unselectedObjects.Contains(ActiveSelection))
                unselectedObjects.Remove(ActiveSelection);

            onSelectionChange.Invoke();

            InvokeSelection(ActiveSelection);

            foreach (T unselectedObject in unselectedObjects)
                InvokeUnselection(unselectedObject);

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
                if (allObjects.IndexOf(removalObject) != SelectionIndex)
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
                else if (allObjects.Count == 0)
                    onSelectionChange.Invoke();
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
            if (IsServer == false) return;

            if (allObjects.Count != 0)
                Select(allObjects[index]);
        }

        public void Select(T gameObject)
        {
            TryInitalize();

            if (IsServer == false) return;

            if (allObjects.Contains(gameObject))
            {
                //Debug.Log("Selecting Item " + gameObject + ", Index Is At: " + selectionIndex);
                SelectionIndex = allObjects.IndexOf(gameObject);
            }
        }

        public void SelectForward()
        {
            TryInitalize();

            if (allObjects.Count != 0)
                Select(allObjects[SelectionIndex.Increase(allObjects)]);
        }

        public void SelectBackward()
        {
            TryInitalize();
            if (allObjects.Count != 0)
                Select(allObjects[SelectionIndex.Decrease(allObjects)]);
        }

        public void TryInitalize()
        {
            if (isInitalized == false)
                Initalize(allObjects);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return allObjects.GetEnumerator();
        }
    }
}

#endif
