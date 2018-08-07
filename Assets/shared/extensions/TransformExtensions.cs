using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shared.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Check if the target is in line of sight
        /// </summary>
        /// <param name="origin"> Transform Origin</param>
        /// <param name="target"> Target Direction</param>
        /// <param name="fieldOfView">Field Of View</param>
        /// <param name="collisionMask">Check against these layers</param>
        /// <param name="offset">Transform Origin Offset for Eyeheight</param>
        /// <returns>yes / no</returns>

        public static bool InLineOfSight(this Transform origin, Vector3 target, float fieldOfView, LayerMask collisionMask, Vector3 offset)
        {
            Vector3 direction = target - origin.position;

            if (Vector3.Angle(origin.forward, direction.normalized) < fieldOfView / 2)
            {
                float distanceToTarget = Vector3.Distance(origin.position, target);

                RaycastHit hit;
                if (Physics.Raycast(origin.position + offset + origin.forward * .3f, direction.normalized, out hit, distanceToTarget, collisionMask))
                {
                    // something blocking view
                    //Debug.DrawLine(transform.position + offi + transform.forward * .3f, direction.normalized + transform.forward * distanceToTarget);
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
