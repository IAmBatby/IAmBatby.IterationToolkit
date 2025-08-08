using System;

public interface IListenOnlyEvent
{
    public void AddListener(Action listener);
    public void RemoveListener(Action listener);
    public void ChangeListener(Action listener, bool value);
}

public interface IListenOnlyEvent<T> : IListenOnlyEvent
{
    public void AddListener(Action<T> listener);
    public void RemoveListener(Action<T> listener);
    public void ChangeListener(Action<T> listener, bool value);
}

public interface IListenOnlyEvent<T,U> : IListenOnlyEvent
{
    public void AddListener(Action<T,U> listener);
    public void RemoveListener(Action<T,U> listener);
    public void ChangeListener(Action<T,U> listener, bool value);
}