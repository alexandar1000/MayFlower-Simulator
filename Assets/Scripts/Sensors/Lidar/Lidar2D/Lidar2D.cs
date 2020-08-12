using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Lidar.Lidar2D
{
    public class Lidar2D : UnityPublisher<SensorMessages::LaserScan>
    {
         public float LaserLength;
        public float RotationPerMinute;
        public int ScanningFrequency;
        protected float RotationStep;
        protected float RotationDuratuion;
        protected Vector3 RotationAxis;
        protected int ScansPerRotation;
        public bool ShowLasers = true;
        public string FrameId = "Unity";
        protected Vector3 BoatDirection;
        protected Vector3 InitialAngle;
        private RotationScan2D _rotationScan;
        protected SensorMessages.LaserScan Message;


        // Start is called before the first frame update
        protected override void Start()
        {

            base.Start();
            RotationStep = RotationPerMinute / 60;
            RotationDuratuion = 1 / RotationStep;
            RotationAxis = transform.up;
            ScansPerRotation = (int) (RotationDuratuion * ScanningFrequency);
            InitialAngle = transform.forward;
            BoatDirection = transform.parent.forward;
            _rotationScan = new RotationScan2D(ScansPerRotation, LaserLength, ShowLasers);
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        void Update()
        {
            transform.Rotate(0, RotationStep * 360 * Time.deltaTime, 0);
            BoatDirection = transform.parent.forward;
        }

        protected void InitialiseMessage()
        {
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

        protected void UpdateMessage()
        {
            // TODO check if direction needs updating to local
            
            // Update the header
            Message.header.Update();
            Vector3 startingPosition = transform.position;
            Vector3 direction = transform.forward;

            // Update the starting angle
            float offsetAngleDegrees = Vector3.SignedAngle(InitialAngle, direction, RotationAxis);
            float offsetAngleRadians = offsetAngleDegrees * Mathf.Deg2Rad;
            Message.angle_min = offsetAngleRadians;
            Message.angle_max = 6.28f - offsetAngleRadians;

            // Scan the points and create the point cloud
            float[] pointsCloud =_rotationScan.Scan(startingPosition, direction);
            Message.ranges = pointsCloud;

            Publish(Message);
        }
    }
}