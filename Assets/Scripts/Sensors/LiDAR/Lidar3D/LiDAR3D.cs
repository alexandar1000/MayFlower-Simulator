using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LiDAR3D : LiDAR
    {
        private RotationScan _rotationScan;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            _rotationScan = new RotationScan(ScansPerRotation, LaserLength);
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        protected override void InitialiseMessage()
        {
            Message = new MessageTypes.Sensor.LaserScan();
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

        protected override void UpdateMessage()
        {
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