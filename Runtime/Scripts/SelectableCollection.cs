using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class SelectableCollection<T> : ISelectableCollection<T>
    {
        [SerializeField] private List<T> allObjects = new List<T>();
        public List<T> Collection => allObjects;

        public int Count => allObjects.Count;

        public bool Contains(T item) => Collection.Contains(item);

        public int SelectedIndex { get; private set; }
        public T Selection => allObjects.IsValidIndex(SelectedIndex) ? allObjects[SelectedIndex] : default;

        private ExtendedEvent<T> onSelected = new ExtendedEvent<T>();
        private ExtendedEvent<T> onUnselected = new ExtendedEvent<T>();
        private ExtendedEvent<T,T> onSelectionChange = new ExtendedEvent<T,T>();

        public IListenOnlyEvent<T, T> OnSelectionChange => onSelectionChange;
        public IListenOnlyEvent<T> OnSelected => onSelected;
        public IListenOnlyEvent<T> OnUnselected => onUnselected;

        public SelectableCollection(List<T> objects = null)
        {
            if (objects != null)
                foreach (T obj in objects)
                    Add(obj);
        }

        public void Add(T newObject, bool selectOnAdd = false)
        {
            if (allObjects.Contains(newObject))
            {
                Debug.LogError("Cannot add object to SelectableCollection as it has already been added.");
                return;
            }

            allObjects.Add(newObject);

            if (selectOnAdd)
                Select(newObject);
        }

        public void Clear()
        {
            //Maybe this should invoke some of the events, im not too sure.
            allObjects.Clear();
            SelectedIndex = -1;
        }

        public void Remove(T removalObject)
        {
            if (removalObject == null || !allObjects.Contains(removalObject)) return;

            int index = allObjects.IndexOf(removalObject);

            allObjects.RemoveAt(index);
            if (index == SelectedIndex)
            {
                if (allObjects.Count == 0)
                    Select(-1);
                else
                    Select(allObjects.Last());
            }
        }

        public void Select(T value)
        {
            if (ValidateInput(value))
                Select(allObjects.IndexOf(value));
        }

        public void Unselect() => Select(-1);

        public void Select(int index)
        {
            if (ValidateInput(index) == false || (index > -1 && ValidateInput(allObjects[index]) == false)) return;

            T previousSelection = Selection;
            SelectedIndex = index;

            onSelectionChange.Invoke(previousSelection,Selection);
            onUnselected.Invoke(previousSelection);
            onSelected.Invoke(Selection);
        }

        public void SelectForward() => Select(SelectedIndex.Increase(allObjects));
        public void SelectBackward() => Select(SelectedIndex.Decrease(allObjects));

        public IEnumerator<T> GetEnumerator() => allObjects.GetEnumerator();

        private bool ValidateInput(int index)
        {
            if (-1 > index || index >= allObjects.Count)
            {
                //Debug.LogError("Cannot select index as it is invalid for the current collection.");
                return (false);
            }
            return (true);
        }

        private bool ValidateInput(T value)
        {
            if (!allObjects.Contains(value))
            {
                //Debug.LogError("Cannot select " + value + " as it is not included in SelectableCollection.");
                return (false);
            }
            return (true);
        }
    }
}
