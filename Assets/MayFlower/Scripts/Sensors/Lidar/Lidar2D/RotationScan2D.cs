using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MayflowerSimulator.Sensors.Lidar;

namespace MayflowerSimulator.Sensors.Lidar.Lidar2D
{
    public class RotationScan2D
    {
        protected int NumberOfLasers;
        protected bool ShowLasers;
        protected float LaserLength;
        protected Laser Laser;
        protected float AngleDifference;
        protected Vector3  CurrDir;

        public RotationScan2D(int numberOfLasers, float laserLength, bool showLasers = false) 
        {
            this.NumberOfLasers = numberOfLasers;
            this.ShowLasers = showLasers;
            this.LaserLength = laserLength;
            this.Laser = new Laser(laserLength);
            this.AngleDifference = 360f / numberOfLasers;
        }

        // Scan all the points at once instead of invoking the method really frequently
        public float[] Scan(Transform localTransform) 
        {
            float[] distances = new float[NumberOfLasers];
            CurrDir = localTransform.forward;

            for (int i = 0; i < NumberOfLasers; i++)
            {
                distances[i] = Laser.ShootLaserForDistance(localTransform, CurrDir, ShowLasers);
                Quaternion offsetAngle = Quaternion.AngleAxis(AngleDifference, localTransform.up);
                CurrDir =  offsetAngle * CurrDir;
            }

            return distances;
            
        }
    }
}
