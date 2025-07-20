using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IHighlightable
{
    public void OnHighlightChanged(bool value) { }
    public bool IsHighlightable() => true;
}
