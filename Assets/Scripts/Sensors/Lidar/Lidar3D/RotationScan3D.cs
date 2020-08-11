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
            // TODO: Test the constructor

        }


        // Scan all the points at once instead of invoking the method really frequently
        public float[] Scan(Vector3 startPosition, Vector3 direction) 
        {
            float[] distances = new float[NumberOfLasersHorizontally];
            CurrDir = direction;

            for (int i = 0; i < NumberOfLasersHorizontally; i++)
            {
                distances[i] = Laser.ShootLaser(startPosition, CurrDir, true);
                // TODO: This Vector3.up might not work always; check it out
                Quaternion offsetAngle = Quaternion.AngleAxis(AngleDifferenceHorizontal, Vector3.up);
                CurrDir =  offsetAngle * CurrDir;
            }

            return distances;
            
        }
}
}
