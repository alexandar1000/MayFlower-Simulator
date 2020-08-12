using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Lidar.Lidar3D
{    
    public class RotationScan3D
    {
        protected int NumberOfLasersHorizontally;
        protected int NumberOfLasersVertically;
        protected bool ShowLaser;
        protected float LaserLength;
        protected Laser Laser;
        protected float AngleDifferenceHorizontal;
        protected float AngleDifferenceVertical;
        protected float UpperAngleBound;
        protected float LowerAngleBound;
        protected float VerticalViewSpan;
        protected Vector3  CurrDir;

        public RotationScan3D(int numberOfLasersHorizontally, int numberOfLasersVertically, float laserLength, float upperAngleBound, float LowerAngleBound, bool showLaser=true) 
        {
            this.NumberOfLasersHorizontally = numberOfLasersHorizontally;
            this.ShowLaser = showLaser;
            this.LaserLength = laserLength;
            this.Laser = new Laser(laserLength);
            this.AngleDifferenceHorizontal = 360f / numberOfLasersHorizontally;
            this.NumberOfLasersVertically = numberOfLasersVertically;
            this.UpperAngleBound = upperAngleBound;
            this.LowerAngleBound = LowerAngleBound;
            this.VerticalViewSpan = Mathf.Abs(UpperAngleBound - LowerAngleBound);
            this.AngleDifferenceVertical = VerticalViewSpan / numberOfLasersVertically;
        }


        // Scan all the points at once instead of invoking the method really frequently
        public Vector3[ , ] Scan(Transform localTransform) 
        {
            Vector3 startPosition = localTransform.position;
            Vector3 direction = localTransform.forward;

            Vector3[ , ] points = new Vector3[NumberOfLasersVertically, NumberOfLasersHorizontally];
            CurrDir = direction;
            Quaternion offsetAngleVertiacally = Quaternion.AngleAxis(UpperAngleBound, localTransform.right);
            CurrDir =  offsetAngleVertiacally * CurrDir;

            for (int i = 0; i < NumberOfLasersVertically; i++)
            {
                Quaternion AngleDecrementVertiacally = Quaternion.AngleAxis(-AngleDifferenceVertical, localTransform.right);
                CurrDir =  AngleDecrementVertiacally * CurrDir;
                        
                for (int j = 0; j < NumberOfLasersHorizontally; j++)
                {
                    Vector3 globalPoint = Laser.ShootLaserForPoint(startPosition, CurrDir, true);
                    points[i, j] = globalPoint;
                    Quaternion offsetAngle = Quaternion.AngleAxis(AngleDifferenceHorizontal, localTransform.up);
                    CurrDir =  offsetAngle * CurrDir;
                }
            }

            return points;
            
        }
}
}
