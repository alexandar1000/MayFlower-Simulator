using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TemperatureSensor : UnityPublisher<MessageTypes.Std.Float64>
    {
        public float MeasurementFrequency = 2f;
        public GameObject[] TemperatureAnomalies;

        public float GlobalTemperature = 25f;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InvokeRepeating("MeasureTemperature", MeasurementFrequency, MeasurementFrequency);  //1s delay, repeat every 1s
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
                    if (GlobalTemperature > temperature) {
                        change *= -1;
                    }

                    // Formula for calculating the influence of the temperature anomaly on the current reading
                    temperatureReading += change;
                }
            }
            Publish(PrepareMessage(temperatureReading));
        }

        private MessageTypes.Std.Float64 PrepareMessage(float temperature)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = temperature;
            
            return message;
        }
    }
}