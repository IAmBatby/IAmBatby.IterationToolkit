using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StyleStates
{
    [field: SerializeField] public StyleStateColors Normal { get; private set; }
    [field: SerializeField] public StyleStateColors Hover { get; private set; }
    [field: SerializeField] public StyleStateColors Focused { get; private set; }
    [field: SerializeField] public StyleStateColors Active { get; private set; }
}
