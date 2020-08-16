using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class PowerMessage : UnityPublisher<MessageTypes.Std.Float64>
    {
        public float MeasurementFrequency = 2f;
        private static float power;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InvokeRepeating("MeasurePower", MeasurementFrequency, MeasurementFrequency);  //1s delay, repeat every 1s
        }

        // Get the current battery power
        private void MeasurePower()
        {
            power = Battery.power;
            Publish(PrepareMessage(power));
        }

        private MessageTypes.Std.Float64 PrepareMessage(float batteryPower)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = batteryPower;

            return message;
        }
    }
}