using IterationToolkit;
using UnityEngine;

[System.Serializable]
public class ScriptableSetting<T> : ScriptableSetting
{
    protected ScriptableSetting<T> RuntimeSettings
    {
        get
        {
            if (runtimeSetting == null)
                CreateRuntimeCopy();
            return (runtimeSetting as ScriptableSetting<T>);
        }
    }

    [SerializeField] protected T _settingValue;
    public T Value
    {
        get => RuntimeSettings._settingValue;
        set => RuntimeSettings.SetValue(value);
    }

    protected void SetValue(T newValue)
    {
        T oldValue = RuntimeSettings._settingValue;
        RuntimeSettings._settingValue = newValue;
        OnValueChanged.Invoke((oldValue, newValue));
        OnChanged.Invoke();
    }


    public ExtendedEvent<(T oldValue, T newValue)> OnValueChanged = new ExtendedEvent<(T oldValue, T newValue)>();
}

public class ScriptableSetting : ScriptableObject
{
    public ExtendedEvent OnChanged = new ExtendedEvent();

    [SerializeField] protected ScriptableSetting runtimeSetting;

    protected void CreateRuntimeCopy()
    {
        runtimeSetting = Instantiate(this);
    }
}

