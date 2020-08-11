using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Lidar.Lidar3D
{
    public class Lidar3D : UnityPublisher<SensorMessages::PointCloud2>
    
    {
        public float LaserLength;
        public float RotationPerMinute;
        public int ScanningFrequency;
        public float UpperAngleBound;
        public float LowerAngleBound;
        protected float RotationStep;
        protected float RotationDuratuion;
        protected Vector3 RotationAxis;
        protected int ScansPerRotation;
        public bool ShowLasers = true;
        public string FrameId = "Unity";
        protected Vector3 BoatDirection;
        protected Vector3 InitialAngle;
        private RotationScan3D _rotationScan;
        protected SensorMessages.PointCloud2 Message;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            _rotationScan = new RotationScan3D(ScansPerRotation, LaserLength, UpperAngleBound, LowerAngleBound, ShowLasers);
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

        }

        protected void UpdateMessage()
        {

        }

    }
}