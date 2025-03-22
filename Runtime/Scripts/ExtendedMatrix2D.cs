using PlasticGui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtendedMatrix2D<T> where T : UnityEngine.Object
{
    [SerializeField] private List<(Vector2Int index, T value)> flattenedMatrix = new List<(Vector2Int index, T value)>();
    [field: SerializeField] public Vector2Int Bounds { get; private set; }
    private T[,] _matrix;

    private T[,] Matrix
    {
        get
        {
            if (_matrix == null)
            {
                _matrix = new T[Bounds.x,Bounds.y];
                foreach ((Vector2Int index, T value) pair in flattenedMatrix)
                    _matrix[pair.index.x,pair.index.y] = pair.value;
            }
            return (_matrix);
        }
    }



    public ExtendedMatrix2D(Vector2Int length)
    {
        Bounds = length;
    }

    public void Clear()
    {
        _matrix = null;
        ReconstructFlattenedMatrix();
    }

    public void Replace(T[,] newMatrix)
    {
        _matrix = newMatrix;
        ReconstructFlattenedMatrix();
    }

    public T Get(Vector2Int index) => Matrix[index.x, index.y];
    public T Get(int x, int y) => Matrix[x, y];

    public bool TryGet(Vector2Int index, out T returnValue) => TryGet(index, out returnValue);
    public bool TryGet(int x, int y, out T returnValue)
    {
        returnValue = null;
        if (IsValidIndex(x, y))
            returnValue = Matrix[x, y];
        return (returnValue != null);
    }

    public void Set(Vector2Int index, T value) => Set(index.x, index.y, value);
    public void Set(int x, int y, T value) => ModifyMatrix(x, y, value);

    public bool TrySet(Vector2Int index, T value) => TrySet(index.x, index.y, value);
    public bool TrySet(int x, int y, T value)
    {
        bool result = IsValidIndex(x, y);
        if (result) Set(x,y, value);
        return (result);
    }

    public bool IsValidIndex(Vector2Int index) => IsValidIndex(index.x,index.y);
    public bool IsValidIndex(int x, int y) => (x > -1 && y > -1 && x < Bounds.x && y < Bounds.y);

    private void ModifyMatrix(int x, int y, T value)
    {
        if (Matrix[x, y].Equals(value)) return;
        Matrix[x, y] = value;
        ReconstructFlattenedMatrix();
    }

    private void ReconstructFlattenedMatrix()
    {
        flattenedMatrix.Clear();
        for (int x = 0; x < Bounds.x; x++)
            for (int y = 0; y < Bounds.y; y++)
                flattenedMatrix.Add((new Vector2Int(x,y), Matrix[x, y]));
    }
}
