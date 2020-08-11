using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
   public class DistanceLaserSensors : UnityPublisher<MessageTypes.Std.Int64MultiArray>
   {

      public GameObject boat;
      public GameObject canal;
      public float distance;


      // Start is called before the first frame update

      [Header("Sensors")]
      public float sensorLength = 5f;
      public Vector3 frontSensorPosition = new Vector3(0f,0.2f,0.5f);
      public float frontSideSensorPosition = 0.2f;
      public float frontSensorAngle = 30;
      public float backSensorPosition = 0.2f;
      private float sensorNr = 0;

       private float nextActionTime = 0.0f;
      public float period = 0.1f;

/*
      void Start(){
         InvokeRepeating("laserSensors", 0.5f, 0.1f);
      }
      */

      // Update is called once per frame
      void Update()
      {

         if (Time.time > nextActionTime ) {
        nextActionTime += period;
        
         distance = Vector3.Distance(boat.transform.position,canal.transform.position);
         //Debug.Log(sensorNr);
         if (distance < 3.5)
         {
               Debug.Log("ALERT: BOAT CLOSER TO CANAL");
         }
         laserSensors();
     }

      }

      private void laserSensors(){
            RaycastHit hit;
            Vector3 sensorStartPos = transform.position + frontSensorPosition;

            long[] sensors = {0,0,0,0,0,0};

            
            //Front Centre Sensor
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 1;
               sensors[0] = 1;
            }

            //Front Right Side Sensor
            sensorStartPos.x += frontSideSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 2;
               sensors[1] = 1;
            }
            

            //Front Right Angle Sensor
            else if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 3;
               sensors[2] = 1;
            }

            //Front Left Side Sensor
            sensorStartPos.x -= 2 * frontSideSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 4;
               sensors[3] = 1;
            }

            //Front Left Angle Sensor
            else if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 5;
               sensors[4] = 1;
            }

            //Back Sensor
            sensorStartPos.y += backSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward * -1, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 6;
               sensors[5] = 1;
            }

         Debug.Log(sensors[0] + ", " + sensors[1] + ", " + sensors[2] + ", " + sensors[3] + ", " + sensors[4] + ", " + sensors[5]);
            
         //Debug.Log(distance);
         Publish(PrepareMessage(sensors));
            
      }
   private MessageTypes.Std.Int64MultiArray PrepareMessage(long[] msg)
   {
    MessageTypes.Std.Int64MultiArray message = new MessageTypes.Std.Int64MultiArray();
    message.data = msg;

    return message;
   }
   }
}
