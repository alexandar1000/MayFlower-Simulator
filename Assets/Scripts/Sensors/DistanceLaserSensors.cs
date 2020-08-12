using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class DistanceLaserSensors : UnityPublisher<MessageTypes.Std.Float64>
    {

        public GameObject boat;
        public GameObject env;
        public float distance;

        [Header("Laser Sensor")]
        public float sensorLength = 5f;
        public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
        public float frontSideSensorPosition = 0.2f;
        public float frontSensorAngle = 30;
        public float backSensorPosition = 0.2f;


        // Update is called once per frame
        void Update()
        {
            distance = Vector3.Distance(boat.transform.position, env.transform.position);
            if (distance < 3.5)
            {
                Debug.Log("ALERT: BOAT CLOSER TO ENVIRONMENT");
            }
            laserSensors();
        }

        private void laserSensors()
        {
            RaycastHit hit;
            Vector3 sensorStartPos = transform.position + frontSensorPosition;


            //Front Centre Sensor
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

            }

            //Front Right Side Sensor
            sensorStartPos.x += frontSideSensorPosition;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
            }


            //Front Right Angle Sensor
            if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
            }

            //Front Left Side Sensor
            sensorStartPos.x -= 2 * frontSideSensorPosition;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
            }

            //Front Left Angle Sensor
            if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
            }

            //Back Sensor
            sensorStartPos.y += backSensorPosition;
            if (Physics.Raycast(sensorStartPos, transform.forward * -1, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);

            }

            Publish(PrepareMessage(distance));
        }
        private MessageTypes.Std.Float64 PrepareMessage(float laser)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = laser;
            return message;
        }
    }
}
