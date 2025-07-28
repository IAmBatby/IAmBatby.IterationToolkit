using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IterationToolkit
{
    public interface IGrid
    {
        public Transform Source { get; }
        public Vector3Int Range { get; }

        public Vector3 ArrayCenterOffset => new Vector3((float)Range.x / 2, (float)Range.y / 2,(float)Range.z / 2);
        public Vector3 ArrayCenterPosition => (Vector3.zero - ArrayCenterOffset) + (Source.lossyScale / 2);

        public List<GridUnitInfo> GetUnits()
        {
            List<GridUnitInfo> returnList = new List<GridUnitInfo>();

            for (int y = 0; y < Range.y; y++)
            {
                for (int z = 0; z < Range.z; z++)
                {
                    for (int x = 0; x < Range.x; x++)
                    {
                        returnList.Add(new GridUnitInfo(new Vector3Int(x,y,z),IndexToPosition(new Vector3Int(x, y, z), Space.World)));
                    }
                }
            }

            return (returnList);
        }

        public Vector3 IndexToPosition(Vector3Int index, Space spaceType)
        {
            if (spaceType == Space.World)
                return (Source.localToWorldMatrix.MultiplyPoint3x4(index + ArrayCenterPosition));
            else
                return (Source.worldToLocalMatrix.MultiplyPoint3x4(index + ArrayCenterPosition) + Source.position);
        }

        public void DrawAll()
        {
            foreach (GridUnitInfo info in GetUnits())
                if (CanDrawUnit(info))
                    DrawUnit(info);
        }

        public bool CanDrawUnit(GridUnitInfo unit);

        public void DrawUnit(GridUnitInfo unit);
    }

    [System.Serializable]
    public struct GridUnitInfo
    {
        [field: SerializeField] public Vector3Int Index { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }

        public GridUnitInfo(Vector3Int index, Vector3 position)
        {
            Index = index;
            Position = position;
        }
    }
}
