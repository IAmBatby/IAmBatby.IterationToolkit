using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public interface IGridManager<T> where T : IGridInfo
    {
        public Vector3Int Range { get; }
        public bool TryGetGridInfo(Vector3Int sourceIndex, Direction direction, int distance, out T gridInfo);

        public bool IsValidIndex(Vector3Int index);

        public bool TryGetGridInfo(Vector3Int sourceIndex, out T gridInfo);

        public T GetGridInfo(Vector3Int sourceIndex, Direction direction, int distance);
        public T GetGridInfo(Vector3Int index);
    }
}
