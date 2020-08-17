/* 
 Laser class serves as class used by the Lidar implementations for the distance finding. It relies on Raycasting.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Sensors.Lidar
{    
    public class Laser
    {
        protected float LaserLength;
        

        public Laser(float laserLength)
        {
            this.LaserLength = laserLength;
        }

        /*
            Fires a laser with the specified starting position obtained from the passed transform and the direction.
            It is also possible to pass a flag which toggles the visibility of the laser for the debugging purposes.
            The method returns a distance to the hit point of the raycast, or 0 if no hit was registerd.
        */
        public float ShootLaserForDistance(Transform localTransform, Vector3 direction, bool showLasers = false)
        {
            Vector3 startPosition = localTransform.position;
            RaycastHit raycastHit;
            if(showLasers) 
            {
                Debug.DrawRay(startPosition, direction * 40, Color.red);
            }
            if(Physics.Raycast(startPosition, direction, out raycastHit, this.LaserLength)) {
                // Returns a float distance to the hit point
                return Vector3.Distance(startPosition, raycastHit.point);
            }
            return 0f;
        }

        /*
            Fires a laser with the specified starting position obtained from the passed transform and the direction.
            It is also possible to pass a flag which toggles the visibility of the laser for the debugging purposes.
            The method returns a raycast hit point in a local coordinate system, or zero Vetctor3 if no hit was registerd.
        */
        public Vector3 ShootLaserForPoint(Transform localTransform, Vector3 direction, bool showLasers = false)
        {
            Vector3 startPosition = localTransform.position;
            RaycastHit raycastHit;
            if(showLasers) 
            {
                Debug.DrawRay(startPosition, direction * 40, Color.red);
            }
            if(Physics.Raycast(startPosition, direction, out raycastHit, this.LaserLength))
            {
                // Returns a float distance to the hit point
                return localTransform.parent.InverseTransformPoint(raycastHit.point);
            }
            return Vector3.zero;
        }
    }
}
