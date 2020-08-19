using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RosSharp.RosBridgeClient;
using StdMessages = RosSharp.RosBridgeClient.MessageTypes.Std;

//linkt to ros http://docs.ros.org/melodic/api/sensor_msgs/html/msg/MagneticField.html


namespace MayflowerSimulator.Sensors.Compass
{
    public class CompassSensor : UnityPublisher<StdMessages.Float64>
    {
        public Vector3 NorthDirection;
        public Transform Player;
        public static Quaternion MissionDirection;

        public RectTransform Northlayer;
        public RectTransform MissionLayer;

        public Transform missionplace;

        float sensorReading;

        // Update is called once per frame
        void Update()
        {
            ChangeNorthDirection();
            ChangeMissionDirection();
        }

        public void ChangeNorthDirection()
        {
            NorthDirection.z = Player.eulerAngles.y;

            Northlayer.localEulerAngles = NorthDirection;
        }

        public void ChangeMissionDirection()
        
        {


            Vector3 dir = transform.position - missionplace.position;

            MissionDirection = Quaternion.LookRotation(dir);

            MissionDirection.z = -MissionDirection.y;

            MissionDirection.x = 0;

            MissionDirection.y = 0;

            MissionLayer.localRotation = MissionDirection * Quaternion.Euler(NorthDirection);


            if (MissionDirection.y > 0)
            {
                sensorReading = 0;
            }
        else
            {
            sensorReading = 180;
            }
            if (MissionDirection.x > 0)
            {
            sensorReading = 270;
            }
            else
            {
            sensorReading = 90;
            }


            Debug.Log("compass"+sensorReading);
            Publish(PrepareMessage(sensorReading));

        }



        private StdMessages::Float64 PrepareMessage(float compass)
        {
            StdMessages.Float64 message = new StdMessages::Float64();
            message.data = compass;

            return message;
        }
    }

}
