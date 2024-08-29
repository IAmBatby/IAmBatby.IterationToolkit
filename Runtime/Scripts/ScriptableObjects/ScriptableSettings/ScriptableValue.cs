using Codice.Client.Common.GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit
{
    [System.Serializable]
    public abstract class ValueSetting
    {
        public abstract void RefreshValue();
        public abstract void ResetValue();

        private string _displayName;
        public string DisplayName => _displayName;

        public NewScriptableSetting ScriptableSetting { get; set; }
        public ExtendedEvent OnValueChanged = new ExtendedEvent();
    }

    public abstract class ValueSetting<T> : ValueSetting
    {
        [SerializeField] protected T _value;
        [SerializeField] private T _defaultValue;
        protected virtual T RuntimeValue { get; set; }
        public virtual T Value { get { return (RuntimeValue); } set { SetValue(value); } }

        public new ExtendedEvent<T> OnValueChanged = new ExtendedEvent<T>();

        public override void RefreshValue()
        {
            if (Application.isPlaying == false && Compare(_defaultValue, RuntimeValue) == false)
                ResetValue();
            else
                SetValue(GetInternalValue());
        }

        public override void ResetValue()
        {
            OnValueChanged = new ExtendedEvent<T>();
            RuntimeValue = _defaultValue;
            SetValue(_defaultValue);
        }

        protected virtual T GetInternalValue() => _value;
        protected virtual void SetInternalValue(T newValue) { _value = newValue; }

        public void SetValue(T newValue)
        {
            T oldValue = RuntimeValue;
            SetInternalValue(newValue);
            if (Application.isPlaying == false)
                _defaultValue = GetInternalValue();
            RuntimeValue = GetInternalValue();
            if (Compare(oldValue, newValue) == false)
            {
                OnValueChanged.Invoke();
                if (ScriptableSetting != null)
                    ScriptableSetting.ValuesChanged();
                if (oldValue != null && newValue != null)
                    Debug.Log("ValueSetting OnValueChanged! Old Value: " + oldValue + ", New Value: " + newValue);
            }

        }

        public abstract bool Compare(T firstValue, T secondValue);
    }

    [System.Serializable]
    public class BoolSetting : ValueSetting<bool> { public override bool Compare(bool firstValue, bool secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class FloatSetting : ValueSetting<float> { public override bool Compare(float firstValue, float secondValue) => (Mathf.Approximately(firstValue, secondValue)); }

    [System.Serializable]
    public class IntSetting : ValueSetting<int> { public override bool Compare(int firstValue, int secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class StringSetting : ValueSetting<string> { public override bool Compare(string firstValue, string secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class Vector2Setting : ValueSetting<Vector2> { public override bool Compare(Vector2 firstValue, Vector2 secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class Vector3Setting : ValueSetting<Vector3> { public override bool Compare(Vector3 firstValue, Vector3 secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class ColorSetting : ValueSetting<Color> { public override bool Compare(Color firstValue, Color secondValue) => (firstValue == secondValue); }

    [System.Serializable]
    public class ObjectSetting<M> : ValueSetting<Object> where M : Object
    {
        [SerializeField] private M _typeValue;
        public override Object Value { get { return (base.RuntimeValue as M); } set { SetValue(value); } }

        protected override Object GetInternalValue() => _typeValue;
        protected override void SetInternalValue(Object newValue) { _typeValue = newValue as M; _value = newValue; }

        public override bool Compare(Object firstValue, Object secondValue) => (firstValue == secondValue);
    }

    [System.Serializable]
    public class EnumSetting<M> : ValueSetting<Enum> where M : Enum
    {
        [SerializeField] private M _typeValue;
        public override Enum Value
        {
            get
            {
                if (base.RuntimeValue != null)
                    return (M)base.RuntimeValue;
                else
                    return (null);
            }
        }

        protected override Enum GetInternalValue() => _typeValue;

        public override bool Compare(Enum firstValue, Enum secondValue) => (Enum.Equals(firstValue, secondValue));
    }
}
