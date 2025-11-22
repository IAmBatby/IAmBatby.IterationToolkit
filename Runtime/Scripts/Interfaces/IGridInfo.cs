using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public interface IGridInfo
    {
        public Vector3Int Index { get; }
        public Vector3 Position { get; }
    }
}
