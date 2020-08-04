using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class RollMessage : UnityPublisher<MessageTypes.Std.Float64>
    {
        public float MeasurementFrequency = 2f;
        private static float roll;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InvokeRepeating("MeasureRoll", MeasurementFrequency, MeasurementFrequency);  //1s delay, repeat every 1s
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Get the current roll tilting angle
        private void MeasureRoll()
        {
            roll = Rotate.roll;
            UnityEngine.Debug.Log(roll);
            Publish(PrepareMessage(roll));
        }

        private MessageTypes.Std.Float64 PrepareMessage(float rollAngle)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = rollAngle;

            return message;
        }
    }
}