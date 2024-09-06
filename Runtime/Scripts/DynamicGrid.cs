using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class DynamicGrid : MonoBehaviour
    {
        public Vector3Int spawnRange;

        private Vector3 arrayCenterOffset => new Vector3((float)spawnRange.x / 2, (float)spawnRange.y / 2, (float)spawnRange.z / 2);
        private Vector3 arrayCenterPosition => (Vector3.zero - arrayCenterOffset) + (transform.lossyScale / 2);

        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireCube(transform.position + centreOffset, new Vector3(spawnableSize.x, 1, spawnableSize.y));

            foreach (Vector3 position in GetUnits())
                Gizmos.DrawWireCube(position, 1f * transform.lossyScale);
        }

        public List<Vector3> GetUnits()
        {
            List<Vector3> returnList = new List<Vector3>();

            for (int y = 0; y < spawnRange.y; y++)
            {
                for (int z = 0; z < spawnRange.z; z++)
                {
                    for (int x = 0; x < spawnRange.x; x++)
                    {
                        returnList.Add(IndexToPosition(new Vector3Int(x, y, z), Space.World));
                    }
                }
            }

            return (returnList);
        }

        public Vector3 IndexToPosition(Vector3Int index, Space spaceType)
        {
            if (spaceType == Space.World)
                return (transform.localToWorldMatrix.MultiplyPoint3x4(index + arrayCenterPosition));
            else
                return (transform.worldToLocalMatrix.MultiplyPoint3x4(index + arrayCenterPosition) + transform.position);
        }
    }
}
