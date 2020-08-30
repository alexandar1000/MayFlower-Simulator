using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LaserSensors : UnityPublisher<MessageTypes.Sensor.Range>
    {
        public string FrameId = "Unity";
        public GameObject laser;

        [Header("Laser Settings")]
        public string laserType = "straight";
        public float sensorLength = 10f;
        public float frontSensorAngle = 30;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        private MessageTypes.Sensor.Range message;

        protected override void Start()
        {
            base.Start();
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }


        void InitialiseMessage()
        {
            message = new MessageTypes.Sensor.Range();
            message.header = new MessageTypes.Std.Header();
            message.header.frame_id = FrameId;
            message.radiation_type = 1; //infrared
            message.field_of_view = 0;
            message.min_range = 0;
            message.max_range = 2;
            message.range = 11;
        }

        // Update is called once per frame
        void UpdateMessage()
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
                message.header.Update();
                message.range = hit.distance;
                Debug.Log("header stamp secs: "+ message.header.stamp.secs + " distance: "+ message.range);
                //Publish(PrepareMessage(hit.distance));  
                Publish(message);
            }
            
        }

        /*private MessageTypes.Sensor.Range PrepareMessage(float distance)
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
        }*/

    }
}

