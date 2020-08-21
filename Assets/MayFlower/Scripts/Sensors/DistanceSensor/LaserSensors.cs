using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LaserSensors : UnityPublisher<MessageTypes.Sensor.Range>
    {
        public string FrameId = "Unity";
        public GameObject laser;

        [Header("Laser Settings")]
        public string laserType = "straight";
        public float sensorLength = 5f;
        public float frontSensorAngle = 30;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;

            if(laserType == "straight"){
                if (Physics.Raycast(laser.transform.position, laser.transform.forward, out hit, sensorLength))
                {
                    //Debug.Log("front center laser: "+ hit.transform.name + " distance is "+ hit.distance);
                }
            Debug.DrawLine(laser.transform.position, hit.point);
            }
            
            else if(laserType == "rightAngle"){
                if (Physics.Raycast(laser.transform.position, Quaternion.AngleAxis(frontSensorAngle, laser.transform.up) * laser.transform.forward, out hit, sensorLength))
                {
                    //Debug.Log("right angle laser" + hit.transform.name);
                }

            }

            else {
                //Left Angle Sensor
                if (Physics.Raycast(laser.transform.position, Quaternion.AngleAxis(-frontSensorAngle, laser.transform.up) * laser.transform.forward, out hit, sensorLength))
                {
                    //Debug.Log("left angle laser" + hit.transform.name);
                }
            }

            if (Time.time > nextActionTime ) 
            {
                nextActionTime += period;
                Publish(PrepareMessage(hit.distance));  
            }
            
        }

        private MessageTypes.Sensor.Range PrepareMessage(float distance)
        {
            MessageTypes.Sensor.Range message = new MessageTypes.Sensor.Range
            {
                header = new MessageTypes.Std.Header { frame_id = FrameId },
                radiation_type  = 1, //infrared
                field_of_view   = 0,
                min_range       = 0,
                max_range       = 2,
                range           = distance
            };

            return message;
        }

    }
}

