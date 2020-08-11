using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LiDAR : UnityPublisher<MessageTypes.Sensor.LaserScan>
    {
        private Laser _laser; 
        private RotationScan _rotationScan;
        public float LaserLength;
        public float RotationPerMinute;
        public int ScanningFrequency;
        private float _rotationStep;
        private float _rotationDuratuion;
        private Vector3 _rotationAxis;
        private int _scansPerRotation;
        public bool ShowLasers = true;
        public string FrameId = "Unity";
        private MessageTypes.Sensor.LaserScan message;
        Timer myTimer;
        int scanCounter = 0;
        private Vector3 _boatDirection;

        private Vector3 _initialAngle;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            _rotationStep = RotationPerMinute / 60;
            _rotationDuratuion = 1 / _rotationStep;
            _rotationAxis = transform.up;
            _scansPerRotation = (int) (_rotationDuratuion * ScanningFrequency);
            _rotationScan = new RotationScan(_scansPerRotation, LaserLength);
            _initialAngle = transform.forward;
            _boatDirection = transform.parent.forward;
            InitializeMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, _rotationStep * 360 * Time.deltaTime, 0);
            _boatDirection = transform.parent.forward;
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Sensor.LaserScan();
            message.header.frame_id = FrameId;
            message.angle_min = 0f;
            message.angle_max = 6.28f; // Full circle/rotation
            message.angle_increment = 6.28f / _scansPerRotation;
            message.time_increment = 0;
            message.scan_time = 0.1f;
            message.range_min = 0.1f;
            message.range_max = 40f;
            message.ranges = new float[_scansPerRotation];
        }

        private void UpdateMessage()
        {
            // Update the header
            message.header.Update();
            Vector3 startingPosition = transform.position;
            Vector3 direction = transform.forward;

            // Update the starting angle
            float offsetAngleDegrees = Vector3.SignedAngle(_initialAngle, direction, _rotationAxis);
            float offsetAngleRadians = offsetAngleDegrees * Mathf.Deg2Rad;
            message.angle_min = offsetAngleRadians;
            message.angle_max = 6.28f - offsetAngleRadians;

            // Scan the points and create the point cloud
            float[] pointsCloud =_rotationScan.Scan(startingPosition, direction);
            message.ranges = pointsCloud;

            Publish(message);
        }
    }
}
