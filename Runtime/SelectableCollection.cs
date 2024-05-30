using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectableCollection<T>
{
    private bool isInitalized;

    [SerializeField] public List<T> allObjects;
    //public List<T> AllObjects => allObjects;
    private List<T> unselectedObjects;
    public T SelectedObject
    {
        get
        {
            if (allObjects != null || allObjects.Count != 0)
                return (allObjects[selectionIndex]);
            else
                return (default);
        }
    }

    private int selectionIndex;

    public ExtendedEvent<T> onSelected = new ExtendedEvent<T>();
    public ExtendedEvent<T> onUnselected = new ExtendedEvent<T>();

    private Dictionary<T, List<Action>> onSelectedActions;
    private Dictionary<T, List<Action>> onUnselectedActions;

    public SelectableCollection(List<T> objects)
    {
        isInitalized = false;
        Initalize(objects);
    }

    public void Initalize(List<T> objects)
    {
        allObjects = new List<T>(objects);
        unselectedObjects = new List<T>(objects);
        onSelectedActions = new Dictionary<T, List<Action>>();
        onUnselectedActions = new Dictionary<T, List<Action>>();
        onSelected = new ExtendedEvent<T>();
        onUnselected = new ExtendedEvent<T>();

        foreach (T newObject in allObjects)
        {
            onSelectedActions.Add(newObject, new List<Action>());
            onUnselectedActions.Add(newObject, new List<Action>());
        }

        isInitalized = true;
    }

    public void AssignOnSelected(IEnumerable<Action> newSelectedActions)
    {
        TryInitalize();

        foreach (Action action in newSelectedActions)
        {
            T convertedTarget = (T)action.Target;
            if (allObjects.Contains(convertedTarget))
                if (!onSelectedActions[convertedTarget].Contains(action))
                    onSelectedActions[convertedTarget].Add(action);
        }
    }

    public void AssignOnUnselected(IEnumerable<Action> newUnselectedActions)
    {
        foreach (Action action in newUnselectedActions)
        {
            T convertedTarget = (T)action.Target;
            if (allObjects.Contains(convertedTarget))
                if (!onUnselectedActions[convertedTarget].Contains(action))
                    onUnselectedActions[convertedTarget].Add(action);
        }
    }

    private void InvokeSelection(T selectedObject)
    {
        onUnselected.Invoke(selectedObject);
        foreach (Action action in onSelectedActions[selectedObject])
            action.Invoke();
    }

    private void InvokeUnselection(T unselectedObject)
    {
        onUnselected.Invoke(unselectedObject);
        foreach (Action aciton in onUnselectedActions[unselectedObject])
            aciton.Invoke();
    }

    public void Select(int index)
    {
        Select(allObjects[index]);
    }

    public void Select(T gameObject)
    {
        TryInitalize();

        if (allObjects.Contains(gameObject))
        {
            if (!unselectedObjects.Contains(SelectedObject))
                unselectedObjects.Add(SelectedObject);

            selectionIndex = allObjects.IndexOf(gameObject);

            if (unselectedObjects.Contains(SelectedObject))
                unselectedObjects.Remove(SelectedObject);

            InvokeSelection(SelectedObject);

            foreach (T unselectedObject in unselectedObjects)
                    InvokeUnselection(unselectedObject);
        }    
    }

    public void SelectForward()
    {
        TryInitalize();

        Select(allObjects[selectionIndex.Increase(allObjects)]);
    }

    public void SelectBackward()
    {
        TryInitalize();

        Select(allObjects[selectionIndex.Decrease(allObjects)]);
    }

    public void TryInitalize()
    {
        if (isInitalized == false)
            Initalize(allObjects);
    }
}
