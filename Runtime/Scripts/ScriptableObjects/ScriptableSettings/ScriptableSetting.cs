using IterationToolkit;
using UnityEngine;

[System.Serializable]
public class ScriptableSetting<T> : ScriptableSetting
{
    public ExtendedEvent<(T oldValue, T newValue)> OnValueChanged = new ExtendedEvent<(T oldValue, T newValue)>();
    [SerializeField] protected T _settingValue;
    public T Value
    {
        get => _settingValue;
        set => SetValue(value);
    }

    protected void SetValue(T newValue)
    {
        T oldValue = _settingValue;
        _settingValue = newValue;
        OnValueChanged.Invoke((oldValue, newValue));
        OnChanged.Invoke();
    }
}

public class ScriptableSetting : ScriptableObject
{
    public ExtendedEvent OnChanged = new ExtendedEvent();

    private void Awake()
    {
        Debug.Log(name + " says hi");
    }
}

