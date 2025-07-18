using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IHighlightable
{
    public bool HighlightingEnabled { get; }
    public MonoBehaviour Target{ get; }

    public void OnHighlightChanged()
    {
        Debug.Log("Default OnHighlightChanged: " + Target);
    }
}
