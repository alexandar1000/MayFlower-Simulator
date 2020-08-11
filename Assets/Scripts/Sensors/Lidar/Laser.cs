﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Sensors.Lidar
{    
    public class Laser
    {
        private float _laserLength = 25f;
        

        public Laser(float laserLength)
        {
            this._laserLength = laserLength;
        }

        /*
            Fires a laser with the specified starting position and direction.
        */
        public float ShootLaser(Vector3 startPosition, Vector3 direction, bool showLaser=true)
        {
            RaycastHit raycastHit;
            if(showLaser) 
            {
                Debug.DrawRay(startPosition, direction * 40, Color.red);
            }
            if(Physics.Raycast(startPosition, direction, out raycastHit, this._laserLength)) {
                // Returns a float distance to the hit point
                return Vector3.Distance(startPosition, raycastHit.point);
            }
            return 0f;
        }
    }
}