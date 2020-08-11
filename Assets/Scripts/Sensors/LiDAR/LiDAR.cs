using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public abstract class LiDAR : UnityPublisher<MessageTypes.Sensor.LaserScan>
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
        protected MessageTypes.Sensor.LaserScan Message;
        protected Vector3 BoatDirection;
        protected Vector3 InitialAngle;

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
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, RotationStep * 360 * Time.deltaTime, 0);
            BoatDirection = transform.parent.forward;
        }

        protected abstract void InitialiseMessage();
        protected abstract void UpdateMessage();
    }
}
