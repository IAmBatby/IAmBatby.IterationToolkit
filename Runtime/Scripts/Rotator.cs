using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private Vector3 groundHitPosition;

        private Vector3 GroundHitMin => groundHitPosition + new Vector3(0, verticalLerpMinOffset, 0);
        private Vector3 GroundHitMax => GroundHitMin + new Vector3(0, verticalLerpMaxOffset, 0);

        [SerializeField] private float verticalLerpMinOffset;
        [SerializeField] private float verticalLerpMaxOffset;
        [SerializeField] private float verticalLerpSpeed;

        [SerializeField] private float horizontalLerpAmount;
        [SerializeField] private float horizontalLerpSpeed;

        [SerializeField] private bool isRaising;

        [field: SerializeField] public bool EnablePositionChanging;
        [field: SerializeField] public bool EnableRotationChanging;

        private void Awake()
        {
            if (EnablePositionChanging == true)
            {
                if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, groundMask))
                    groundHitPosition = hit.point;
                transform.position = Vector3.Lerp(GroundHitMin, GroundHitMax, 0.5f);
            }
        }

        private void Update() => UpdateRotation();


        //Becuase i'm a freak who sometimes wants to call this from other classes
        public void UpdateRotation()
        {
            if (EnablePositionChanging == true)
            {
                Vector3 target = isRaising == true ? GroundHitMin : GroundHitMax;
                transform.position = Vector3.Lerp(transform.position, target, verticalLerpSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, target) < 0.1f)
                    isRaising = !isRaising;

            }

            if (EnableRotationChanging == true)
                transform.Rotate(new Vector3(0, horizontalLerpAmount, 0));
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(groundHitPosition, new Vector3(0.5f, 0.05f, 0.5f));
            Gizmos.DrawWireCube(GroundHitMin, new Vector3(0.1f, 0.1f, 0.1f));
            Gizmos.DrawWireCube(GroundHitMax, new Vector3(0.1f, 0.1f, 0.1f));
            Gizmos.DrawLine(GroundHitMin, GroundHitMax);
        }
    }
}
