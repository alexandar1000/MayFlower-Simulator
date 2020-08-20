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
        public Environment.Battery.Battery battery;
        public string FrameId;
        protected SensorMessages.BatteryState message;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            float measurementDistanceInSeconds = 1f / MeasurementFrequency;
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, measurementDistanceInSeconds);
        }

        // Update is called once per frame
        void Update()
        {
        }

        protected void InitialiseMessage()
        {
            // Initialise the message fields as per the Battery State documentation
            message = new SensorMessages::BatteryState();
            message.header.frame_id = FrameId;
        }

        /* 
        Update the LaserScan message with the points scanned by the RotationScanner2D before sending it to ROS
        */
        protected void UpdateMessage()
        {
            // Update the header before sending the message
            message.header.Update();

            // Update the remaining fields
            message.power_supply_status = battery.GetCurrentChargingStatus();
            message.percentage = battery.GetCurrentCharge();

            // Publish the message to ROS
            Publish(message);
        }
    }
}