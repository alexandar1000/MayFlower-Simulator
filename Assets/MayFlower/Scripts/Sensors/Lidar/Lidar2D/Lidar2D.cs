/* 
Implementation of the 3D Lidar which is to be attached to an object above the main vessel
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Lidar.Lidar2D
{
    public class Lidar2D : UnityPublisher<SensorMessages::LaserScan>
    {
        public float RotationsPerMinute;
        public int ScanningFrequency;
        public float LaserLength;
        public string FrameId = "Unity";
        public bool ShowLasers = true;
        protected float RotationsPerSecond;
        protected float RotationDuratuion;
        protected Vector3 RotationAxis;
        protected int ScansPerRotation;
        protected Vector3 BoatDirection;
        protected Vector3 InitialAngle;
        protected RotationScan2D RotationScan;
        protected SensorMessages.LaserScan Message;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
                        
            // Inititalise class needed constants
            RotationsPerSecond = RotationsPerMinute / 60;
            RotationDuratuion = 1 / RotationsPerSecond;
            RotationAxis = transform.up;
            ScansPerRotation = (int) (RotationDuratuion * ScanningFrequency);
            BoatDirection = transform.parent.forward;
            
            // Initialise the Lidar's rotating scanner
            RotationScan = new RotationScan2D(ScansPerRotation, LaserLength, ShowLasers);

            // Initialise the message and set the update message method to be called on an interval equal to the duration of the rotation
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, RotationDuratuion);
        }

        void Update()
        {
            // Animate the rotation of the lidar and  update the direction it is facing
            transform.Rotate(0, RotationsPerSecond * 360 * Time.deltaTime, 0);
            BoatDirection = transform.parent.forward;
        }

        /* 
        Initialise the LaserScan message initially
        */
        protected void InitialiseMessage()
        {
            // Initialise the message fields as per the LaserScan documentation
            Message = new SensorMessages::LaserScan();
            Message.header.frame_id = FrameId;
            Message.angle_min = 0f;
            Message.angle_max = 6.28f; // Full circle/rotation
            Message.angle_increment = 6.28f / ScansPerRotation;
            Message.time_increment = 0;
            Message.scan_time = 0.1f;
            Message.range_min = 0.1f;
            Message.range_max = 40f;
            Message.ranges = new float[ScansPerRotation];
        }

        /* 
        Update the LaserScan message with the points scanned by the RotationScanner2D before sending it to ROS
        */
        protected void UpdateMessage()
        {
            Vector3 startPosition = transform.position;
            Vector3 direction = transform.forward;

            // Update the header before sending the message
            Message.header.Update();

            // Update the starting angle
            float offsetAngleDegrees = Vector3.SignedAngle(BoatDirection, direction, RotationAxis);
            float offsetAngleRadians = offsetAngleDegrees * Mathf.Deg2Rad;
            Message.angle_min = offsetAngleRadians;
            Message.angle_max = 6.28f - offsetAngleRadians;

            // Scan the points and create the point cloud
            float[] pointsCloud =RotationScan.Scan(transform);
            Message.ranges = pointsCloud;

            // Publish the message to ROS
            Publish(Message);
        }
    }
}