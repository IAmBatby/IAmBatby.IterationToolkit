using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class CameraTarget : MonoBehaviour
    {
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public void ControlCamera()
        {
            Camera.main.transform.position = Vector3.zero;
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = positionOffset;
            Camera.main.transform.localEulerAngles = rotationOffset;
        }
    }
}
