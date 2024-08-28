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

    public override ValueContainer GetValue()
    {
        ValueContainer returnContainer;
        if (_settingValue is int intValue)
            returnContainer = new ValueContainer(intValue);
        else if (_settingValue is float floatValue)
            returnContainer = new ValueContainer(floatValue);
        else if (_settingValue is string stringValue)
            returnContainer = new ValueContainer(stringValue);
        else if (_settingValue is object objectValue)
            returnContainer = new ValueContainer(objectValue);
        else
            returnContainer = new ValueContainer(ValueContainer.SettingType.None);

        return (returnContainer);
    }


    public ExtendedEvent<(T oldValue, T newValue)> OnValueChanged = new ExtendedEvent<(T oldValue, T newValue)>();
}

public abstract class ScriptableSetting : ScriptableObject
{
    public ExtendedEvent OnChanged = new ExtendedEvent();

    [SerializeField] protected ScriptableSetting runtimeSetting;

    protected void CreateRuntimeCopy()
    {
        runtimeSetting = Instantiate(this);
    }

    public abstract ValueContainer GetValue();
}

