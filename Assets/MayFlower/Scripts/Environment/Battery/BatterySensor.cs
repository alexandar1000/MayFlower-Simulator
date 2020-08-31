using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient;
using MayflowerSimulator.Environment.Battery;


namespace MayflowerSimulator.Sensors.Battery
{
    public class BatterySensor : UnityPublisher<SensorMessages::BatteryState>
    {
        public float MeasurementFrequency = 1f;
        public Environment.Battery.Battery Battery;
        public string FrameId = "Unity";
        protected SensorMessages.BatteryState Message;

        protected override void Start()
        {
            base.Start();
            float measurementDistanceInSeconds = 1f / MeasurementFrequency;
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, measurementDistanceInSeconds);
        }

        protected void InitialiseMessage()
        {
            Message = new SensorMessages::BatteryState();
            Message.header.frame_id = FrameId;
        }

        protected void UpdateMessage()
        {
            Message.header.Update();

            Message.power_supply_status = Battery.GetCurrentChargingStatus();
            Message.percentage = Battery.GetCurrentCharge();
            //Debug.Log("Battery Message: " + Message.header.stamp.secs + ", " + Message.percentage + ", status: " + Message.power_supply_status);
            Publish(Message);
        }
    }
}