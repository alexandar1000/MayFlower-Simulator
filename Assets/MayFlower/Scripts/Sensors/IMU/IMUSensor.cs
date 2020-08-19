using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class IMUSensor : UnityPublisher<MessageTypes.Sensor.Imu>
    {
        private MessageTypes.Sensor.Imu ImuMessage;
        public string FrameId = "IMU_Sensor";
        private double[] zeroArr = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        protected override void Start()
        {
            base.Start();

            UnityEngine.Debug.Log("IMUSensor Start");
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        //Create MessageTypes object
        MessageTypes.Geometry.Quaternion quaterObj(Quaternion quaternion)
        {
            return new MessageTypes.Geometry.Quaternion(Math.Round(quaternion.x, 4), Math.Round(quaternion.y, 4), Math.Round(quaternion.z, 4), Math.Round(quaternion.w, 4));
        }
        MessageTypes.Geometry.Vector3 vector3Obj(Vector3 vector3)
        {
            return new MessageTypes.Geometry.Vector3(Math.Round(vector3.x, 4), Math.Round(vector3.y, 4), Math.Round(vector3.z, 4));
        }

        void InitialiseMessage()
        {
            ImuMessage = new MessageTypes.Sensor.Imu();
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
            //UnityEngine.Debug.Log("IMU Linear Acceleration: (" + ImuMessage.linear_acceleration.x + ", " + ImuMessage.linear_acceleration.y + ", " + ImuMessage.linear_acceleration.z +")");
            Publish(ImuMessage);            
        }
    }
}