using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RosSharp.RosBridgeClient;
using MessageTypes = RosSharp.RosBridgeClient.MessageTypes;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Compass
{
    public class Compass : UnityPublisher<MessageTypes.Std.Float64>
    {
        public Vector3 currentRotation;
        public Quaternion rotation;
        public float degree;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        void Update()
        {
            currentRotation = this.transform.forward;
            rotation = Quaternion.Euler(currentRotation);

            //Get the boats rotation angle degree
            degree = (float)((Mathf.Atan2(this.transform.forward.z, -this.transform.forward.x) / Math.PI) * 180f);
            if(degree < 0) degree += 360f;

            if (Time.time > nextActionTime ) 
            {
                nextActionTime += period;
                Publish(PrepareMessage(degree));  
            }
        }

        private MessageTypes.Std.Float64 PrepareMessage(float angle)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = angle;

            return message;
        }
    }
}