using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ValueContainer
{
    public string ValueName { get; private set; }
    public enum SettingType { Int, Float, Bool, String, Enum, Object, None }
    public SettingType ValueType { get; private set; }
    private int intValue;
    private float floatValue;
    private bool boolValue;
    //public enum enumValue;
    private string stringValue;
    private object objectValue;
    private Enum enumValue;

    public ValueContainer(SettingType setting)
    {
        ValueType = SettingType.None;
        ValueName = string.Empty;

        objectValue = null;
        intValue = -1;
        floatValue = -1;
        boolValue = false;
        stringValue = string.Empty;
        enumValue = null;
    }

    public ValueContainer(object newObjectValue, string newValueName = "UNKNOWN")
    {
        ValueName = newValueName;
        ValueType = SettingType.Object;
        objectValue = newObjectValue;

        intValue = -1;
        floatValue = -1;
        boolValue = false;
        stringValue = string.Empty;
        enumValue = null;
    }

    public ValueContainer(float newFloatValue, string newValueName = "UNKNOWN")
    {
        ValueName = newValueName;
        ValueType = SettingType.Float;
        floatValue = newFloatValue;

        intValue = -1;
        objectValue = null;
        boolValue = false;
        stringValue = string.Empty;
        enumValue = null;
    }

    public ValueContainer(string newStringValue, string newValueName = "UNKNOWN")
    {
        ValueName = newValueName;
        ValueType = SettingType.String;
        stringValue = newStringValue;

        intValue = -1;
        floatValue = -1;
        objectValue = null;
        boolValue = false;
        enumValue = null;
    }

    public ValueContainer(Enum newEnumValue, string newValueName = "UNKNOWN")
    {
        ValueName = newValueName;
        ValueType = SettingType.Enum;
        enumValue = newEnumValue;

        intValue = -1;
        floatValue = -1;
        objectValue = null;
        boolValue = false;
        stringValue = string.Empty;
    }

    public bool TryGetBoolValue(out bool returnBoolValue)
    {
        returnBoolValue = false;
        if (ValueType == SettingType.Bool)
        {
            returnBoolValue = boolValue;
            return true;
        }
        return (false);
    }

    public bool TryGetIntValue(out int returnIntValue)
    {
        returnIntValue = -1;
        if (ValueType == SettingType.Int)
        {
            returnIntValue = intValue;
            return true;
        }
        return (false);
    }

    public bool TryGetFloatValue(out float returnFloatValue)
    {
        returnFloatValue = -1;
        if (ValueType == SettingType.Float)
        {
            returnFloatValue = floatValue;
            return true;
        }
        return (false);
    }

    public bool TryGetStringValue(out string returnStringValue)
    {
        returnStringValue = string.Empty;
        if (ValueType == SettingType.String)
        {
            returnStringValue = stringValue;
            return true;
        }
        return (false);
    }

    public bool TryGetEnumValue(out Enum returnEnumValue)
    {
        returnEnumValue = null;
        if (ValueType == SettingType.Enum)
        {
            returnEnumValue = enumValue;
            return true;
        }
        return (false);
    }


    public bool TryGetObjectValue(out object returnObjectValue)
    {
        returnObjectValue = null;
        if (ValueType == SettingType.Object)
        {
            returnObjectValue = objectValue;
            return true;
        }
        return (false);
    }
}
