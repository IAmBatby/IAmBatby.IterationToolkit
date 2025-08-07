#if NETCODE_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    [System.Serializable]
    public class NetworkSelectableCollection<T> : NetworkVariable<int>, ISelectableCollection<T>
    {
        [SerializeField] private List<T> allObjects = new List<T>();
        public List<T> Collection => allObjects;

        public bool IsServer => NetworkManager.Singleton.IsServer;

        public int SelectedIndex { get { return Value; } set { Value = value; } }
        public T Selection => allObjects.IsValidIndex(SelectedIndex) ? allObjects[SelectedIndex] : default;


        private ExtendedEvent<T> onSelected = new ExtendedEvent<T>();
        private ExtendedEvent<T> onUnselected = new ExtendedEvent<T>();
        private ExtendedEvent<T, T> onSelectionChange = new ExtendedEvent<T, T>();

        public IListenOnlyEvent<T, T> OnSelectionChange => onSelectionChange;
        public IListenOnlyEvent<T> OnSelected => onSelected;
        public IListenOnlyEvent<T> OnUnselected => onUnselected;


        public NetworkSelectableCollection(int value = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(value, readPerm, writePerm)
        {
            OnValueChanged += OnCollectionValueChanged;
        }

        private void OnCollectionValueChanged(int prevValue, int newValue)
        {
            T previousSelection = (prevValue > -1 || prevValue < allObjects.Count - 1) ? allObjects[prevValue] : default;

            onSelectionChange.Invoke(previousSelection, Selection);
            onUnselected.Invoke(previousSelection);
            onSelected.Invoke(Selection);
        }

        public void Add(List<T> objects)
        {
            foreach (T obj in objects)
                if (obj != null)
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

        public void Remove(T removalObject)
        {
            if (removalObject == null || !allObjects.Contains(removalObject)) return;

            if (allObjects.IndexOf(removalObject) == SelectedIndex)
                Select(-1);
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
            if (IsServer)
                SelectedIndex = index;
        }

        public void SelectForward() => Select(allObjects[SelectedIndex.Increase(allObjects)]);
        public void SelectBackward() => Select(allObjects[SelectedIndex.Decrease(allObjects)]);

        public IEnumerator<T> GetEnumerator() => allObjects.GetEnumerator();

        private bool ValidateInput(int index)
        {
            if (-1 > index || index >= allObjects.Count)
            {
                Debug.LogError("Cannot select index as it is invalid for the current collection.");
                return (false);
            }
            return (true);
        }

        private bool ValidateInput(T value)
        {
            if (!allObjects.Contains(value))
            {
                Debug.LogError("Cannot select " + value + " as it is not included in SelectableCollection.");
                return (false);
            }
            return (true);
        }

    }
}

#endif
