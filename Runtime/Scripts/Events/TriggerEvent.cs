using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IterationToolkit
{
    [System.Serializable]
    public class UnityEventCollider : UnityEvent<Collider>
    {
    }

    public class TriggerEvent : MonoBehaviour
    {
        private Collider triggerCollider;

        public UnityEventCollider onTriggerEnter;
        public UnityEventCollider onTriggerStay;
        public UnityEventCollider onTriggerExit;

        public Color gizmosColor;

        private void Awake()
        {
            /*
            TriggerEnterEvent triggerEnterForwarder;
            TriggerStayEvent triggerStayForwarder;
            TriggerExitEvent triggerExitForwarder;

            if (onTriggerEnter.GetPersistentEventCount() > 0)
            {
                triggerEnterForwarder = gameObject.AddComponent<TriggerEnterEvent>();
                triggerEnterForwarder.triggerEvent = this;
            }

            if (onTriggerStay.GetPersistentEventCount() > 0)
            {
                triggerStayForwarder = gameObject.AddComponent<TriggerStayEvent>();
                triggerStayForwarder.triggerEvent = this;
            }


            if (onTriggerExit.GetPersistentEventCount() > 0)
            {
                triggerExitForwarder = gameObject.AddComponent<TriggerExitEvent>();
                triggerExitForwarder.triggerEvent = this;
            }
            */
        }

        public void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other);
        }

        public void OnTriggerStay(Collider other)
        {
            onTriggerStay.Invoke(other);
        }

        public void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke(other);
        }

        private void OnDrawGizmosSelected()
        {
            if (triggerCollider == null)
                foreach (Collider collider in gameObject.GetComponents<Collider>())
                    if (collider.isTrigger == true)
                        triggerCollider = collider;

            if (triggerCollider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
                Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.25f);
                Gizmos.DrawCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
            }
        }
    }
}