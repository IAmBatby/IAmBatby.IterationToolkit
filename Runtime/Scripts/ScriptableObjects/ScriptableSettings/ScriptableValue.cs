using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit
{
    [System.Serializable]
    public class ValueSetting { }

    public class ValueSetting<T> : ValueSetting
    {
        [SerializeField] private T _value;
        public virtual T Value => _value;
    }

    public class ValueSetting<T,M> : ValueSetting
    {
        [SerializeField] private T _value;
        public virtual T Value => _value;
    }

    [System.Serializable]
    public class BoolSetting : ValueSetting<bool> { }

    [System.Serializable]
    public class FloatSetting : ValueSetting<float> { }

    [System.Serializable]
    public class IntSetting : ValueSetting<int> { }

    [System.Serializable]
    public class StringSetting : ValueSetting<string> { }

    public abstract class ValueUnityObjectSetting : ValueSetting<Object> { }

    [System.Serializable]
    public class ObjectSetting<M> : ValueSetting<Object> where M : Object
    {
        [SerializeField] private M _typeValue;
        public override Object Value => _typeValue;
    }


    [System.Serializable]
    public class UnityObjectSetting : ValueSetting<Object> { }

    [System.Serializable]
    public class Vector2Setting : ValueSetting<Vector2> { }

    [System.Serializable]
    public class Vector3Setting : ValueSetting<Vector3> { }

    [System.Serializable]
    public class ColorSetting : ValueSetting<Color> { }
}
