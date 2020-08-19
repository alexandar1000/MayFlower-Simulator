using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using RosSharp.RosBridgeClient;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using GeometryMessages = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using MayflowerSimulator.Sensors.Compass;

namespace MayflowerSimulator.Sensors.IMU
{
    public class IMUSensor : UnityPublisher<SensorMessages.Imu>
    {
        private SensorMessages.Imu ImuMessage;
        public string FrameId = "IMU_Sensor";
        private double[] zeroArr = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        protected override void Start()
        {
            base.Start();

            UnityEngine.Debug.Log("IMUSensor Start");
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        // Create MessageTypes object
        GeometryMessages.Quaternion quaterObj(Quaternion quaternion)
        {
            return new GeometryMessages.Quaternion(Math.Round(quaternion.x, 4), Math.Round(quaternion.y, 4), Math.Round(quaternion.z, 4), Math.Round(quaternion.w, 4));
        }
        GeometryMessages.Vector3 vector3Obj(Vector3 vector3)
        {
            return new GeometryMessages.Vector3(Math.Round(vector3.x, 4), Math.Round(vector3.y, 4), Math.Round(vector3.z, 4));
        }

        void InitialiseMessage()
        {
            ImuMessage = new SensorMessages.Imu();
            ImuMessage.header.frame_id = FrameId;
            ImuMessage.orientation = quaterObj(CompassSensor.MissionDirection);
            ImuMessage.linear_acceleration = vector3Obj(IMU.Accelerate_Linear);
            ImuMessage.angular_velocity = vector3Obj(IMU.currentAngularVelocity);
            ImuMessage.orientation_covariance = zeroArr;
            ImuMessage.linear_acceleration_covariance = zeroArr;
            ImuMessage.angular_velocity_covariance = zeroArr;
        }
        

        void UpdateMessage()
        {
            ImuMessage.header.Update();
            ImuMessage.orientation = quaterObj(CompassSensor.MissionDirection);
            ImuMessage.linear_acceleration = vector3Obj(IMU.Accelerate_Linear);
            ImuMessage.angular_velocity = vector3Obj(IMU.currentAngularVelocity);
            Publish(ImuMessage);            
        }
    }
}