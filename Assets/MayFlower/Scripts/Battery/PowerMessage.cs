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

        // Update is called once per frame
        void Update()
        {

        }

        // Get the current battery power
        private void MeasurePower()
        {
            power = Battery.power;
            //UnityEngine.Debug.Log(power);
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