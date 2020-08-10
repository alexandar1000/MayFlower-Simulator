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
        private float _rotationDuratuion;
        private int _scansPerRotation;
        public bool ShowLasers = true;
        public string FrameId = "Unity";
        private MessageTypes.Sensor.LaserScan message;
        Timer myTimer;
        int scanCounter = 0;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // _laser = new Laser(LaserLength);
            _rotationDuratuion = 1 / (RotationPerMinute / 60);
            // _scansPerRotation = (int) (_rotationDuratuion * ScanningFrequency);
            _scansPerRotation = 1000;
            _rotationScan = new RotationScan(_scansPerRotation, LaserLength);
            // Debug.Log((float) 1 / ScanningFrequency);
            // Debug.Log(_rotationDuratuion);
            // Debug.Log(_scansPerRotation);
            InitializeMessage();
            // Time.fixedDeltaTime = 1 / ScanningFrequency;
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            // transform.Rotate(0, _rotationDuratuion * 360 * Time.deltaTime, 0);
            // Vector3 startingPosition = transform.position;
            // Vector3 direction = transform.forward;
            // _laser.ShootLaser(transform.position, transform.forward, ShowLasers);
        }

        // void FixedUpdate() {
            // Debug.Log("Now");
            // UpdateMessage();
        // }

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
            message.header.Update();
            Vector3 startingPosition = transform.position;
            Vector3 direction = transform.forward;
            float[] pointsCloud =_rotationScan.Scan(startingPosition, direction);
            message.ranges = pointsCloud;
            // message.ranges[scanCounter] = _laser.ShootLaser(startingPosition, direction, ShowLasers);

            // Timer myTimer = new Timer();
            // myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            // myTimer.Interval = 1000; // 1000 ms is one second
            // myTimer.Start();

            // scanCounter++;
            // if(scanCounter == _scansPerRotation) {
            Publish(message);
                // scanCounter = 0;
            // }
            // Debug.Log(pointsCloud);
        }
    }
}
