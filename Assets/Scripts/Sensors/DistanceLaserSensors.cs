using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class DistanceLaserSensors : UnityPublisher<MessageTypes.Std.Int64MultiArray>
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

        private float nextActionTime = 0.0f;
        public float period = 0.1f;


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

      private void laserSensors(){
            RaycastHit hit;
            Vector3 sensorStartPos = transform.position;
            sensorStartPos += transform.forward * frontSensorPosition.z;
            sensorStartPos += transform.up * frontSensorPosition.y;

            long[] sensors = {0,0,0,0,0,0};
            
            //Front Centre Sensor
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[0] = 1;
            }

            //Front Right Side Sensor
            sensorStartPos += transform.right * frontSideSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[1] = 1;
            }
            

            //Front Right Angle Sensor
            if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[2] = 1;
            }

            //Front Left Side Sensor
            sensorStartPos -= transform.right * frontSideSensorPosition * 2;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[3] = 1;
            }

            //Front Left Angle Sensor
            if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[4] = 1;
            }

            //Back Sensor
            sensorStartPos.y += backSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward * -1, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensors[5] = 1;

            }
            
            if (Time.time > nextActionTime ) {
               nextActionTime += period;
               Publish(PrepareMessage(sensors));  
            }
            
      }
        private MessageTypes.Std.Int64MultiArray PrepareMessage(long[] msg)
        {
            MessageTypes.Std.Int64MultiArray message = new MessageTypes.Std.Int64MultiArray();
            message.data = msg;

            return message;
        }
    }
}
