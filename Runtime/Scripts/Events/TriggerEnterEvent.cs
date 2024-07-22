using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class TriggerEnterEvent : MonoBehaviour
    {
        [HideInInspector] public TriggerEvent triggerEvent;

        public void OnTriggerEnter(Collider other)
        {
            triggerEvent.onTriggerEnter.Invoke(other);
            Debug.Log("freak");
        }
    }
}
