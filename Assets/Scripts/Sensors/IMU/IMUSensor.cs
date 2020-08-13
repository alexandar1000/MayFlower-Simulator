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
        private MessageTypes.Std.Header header;
        private static Quaternion MissionDirection;
        private static Vector3 Accelerate_Linear;
        private static Vector3 currentAngularVelocity;    
        private double[] zeroArr = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        void Start()
        {
            //header = new MessageTypes.Std.Header();
            UnityEngine.Debug.Log("IMUSensor Start");
            ImuMessage = new MessageTypes.Sensor.Imu();
        }
        void Update()
        {
            //Message.header.Update();
            header = new MessageTypes.Std.Header();

            //MessageTypes.Std.Header header = new MessageTypes.Std.Header(seq, new Time(), "IMU_Sensor");
            MessageTypes.Geometry.Quaternion orientation = new MessageTypes.Geometry.Quaternion(MissionDirection.x, MissionDirection.y, MissionDirection.z, MissionDirection.w);
            MessageTypes.Geometry.Vector3 linear_acceleration = new MessageTypes.Geometry.Vector3(Accelerate_Linear.x, Accelerate_Linear.y, Accelerate_Linear.z);
            MessageTypes.Geometry.Vector3 angular_velocity = new MessageTypes.Geometry.Vector3(currentAngularVelocity.x, currentAngularVelocity.y, currentAngularVelocity.z);
            
            ImuMessage = new MessageTypes.Sensor.Imu(header, orientation, zeroArr, linear_acceleration, zeroArr, angular_velocity, zeroArr);
           
            Publish(PrepareMessage(ImuMessage));            
        }

        private MessageTypes.Sensor.Imu PrepareMessage(MessageTypes.Sensor.Imu message)
        { 
            return message;
        }
    }
}