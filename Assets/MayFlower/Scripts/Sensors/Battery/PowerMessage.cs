using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using StdMessages = RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Battery
{
    public class PowerMessage : UnityPublisher<StdMessages::Float64>
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
            // TODO: As this file is getting removed, for purposes of solving conflicts, the Battery.power is temporarily being replaced by a constant
            power = 100; //Battery.power;
            Publish(PrepareMessage(power));
        }

        private StdMessages::Float64 PrepareMessage(float batteryPower)
        {
            StdMessages::Float64 message = new StdMessages::Float64();
            message.data = batteryPower;

            return message;
        }
    }
}