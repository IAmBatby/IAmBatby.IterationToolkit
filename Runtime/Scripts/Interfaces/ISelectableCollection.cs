using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectableCollection
{
    public int SelectedIndex { get; }


    public void Select(int index);
    public void Unselect();

    public void SelectForward();
    public void SelectBackward();
}

public interface ISelectableCollection<T> : ISelectableCollection
{
    public T Selection { get; }

    public IListenOnlyEvent<T,T> OnSelectionChange { get; }
    public IListenOnlyEvent<T> OnSelected { get; }
    public IListenOnlyEvent<T> OnUnselected { get; }

    public void Add(T item, bool selectOnAddition = false);
    public void Remove(T item);

    public void Select(T item);

    public IEnumerator<T> GetEnumerator();
}