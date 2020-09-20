using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RosSharp.RosBridgeClient;
using StdMessages = RosSharp.RosBridgeClient.MessageTypes.Std;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using MayflowerSimulator.Environment.Temperature;

namespace RosSharp.RosBridgeClient
{
    public class TemperatureMessage : UnityPublisher<MessageTypes.Sensor.Temperature>
    {
        public float MeasurementFrequency = 2f;
        public GameObject[] TemperatureAnomalies;

        public float GlobalTemperature = 25f;

        private string FrameId = "Unity";
        protected MessageTypes.Sensor.Temperature Message;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            initialiseMessage();
            InvokeRepeating("MeasureTemperature", MeasurementFrequency, MeasurementFrequency);  //1s delay, repeat every 1s
        }

        void initialiseMessage()
        {
            Message = new MessageTypes.Sensor.Temperature();
            Message.header.frame_id = FrameId;
            Message.temperature = 25f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Extract the temperature from the environment
        private void MeasureTemperature()
        {
            float temperatureReading = GlobalTemperature;
            // For simplicity, assume that over a distance of 1 unit, a 1 degree of temperature is lost
            foreach (GameObject item in TemperatureAnomalies)
            {
                // Get anomaly's temperature
                float temperature = item.GetComponent<TemperatureAnomaly>().Temperature;
                // Get the distance from the anomaly
                float distance = Vector3.Distance(gameObject.transform.position, item.transform.position);

                // Difference betwee the global temperature and the anomaly's temperature
                float temperatureDifference = Math.Abs(GlobalTemperature - temperature);

                // If the temperature did not equalize up to the current position of the sensor, take it into consideration
                if (distance < temperatureDifference)
                {
                    float change = temperatureDifference - distance;

                    // Adjust the temperature in regards whether it should be lowered or increased
                    if (GlobalTemperature > temperature)
                    {
                        change *= -1;
                    }

                    // Formula for calculating the influence of the temperature anomaly on the current reading
                    temperatureReading += change;
                }
            }

            Message.header.Update();
            Message.temperature = temperatureReading;
            Debug.Log("Temperature is "+ Message.temperature + ";");
            Publish(Message);
        }
    }
}
