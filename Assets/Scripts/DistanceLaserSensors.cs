using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
   public class DistanceLaserSensors : UnityPublisher<MessageTypes.Std.Float64>
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


      // Update is called once per frame
      void Update()
      {
         distance = Vector3.Distance(boat.transform.position,canal.transform.position);
         Debug.Log(sensorNr);
         if (distance < 3.5)
         {
               Debug.Log("ALERT: BOAT CLOSER TO CANAL");
         }
         laserSensors();
      }

      private void laserSensors(){
            RaycastHit hit;
            Vector3 sensorStartPos = transform.position + frontSensorPosition;

            
            //Front Centre Sensor
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 1;
            }

            //Front Right Side Sensor
            sensorStartPos.x += frontSideSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 2;
            }
            

            //Front Right Angle Sensor
            if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 3;
            }

            //Front Left Side Sensor
            sensorStartPos.x -= 2 * frontSideSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 4;
            }

            //Front Left Angle Sensor
            if(Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 5;
            }

            //Back Sensor
            sensorStartPos.y += backSensorPosition;
            if(Physics.Raycast(sensorStartPos, transform.forward * -1, out hit, sensorLength)){
               Debug.DrawLine(sensorStartPos, hit.point);
               sensorNr = 6;
            }
            
         Debug.Log(distance);
         Publish(PrepareMessage(sensorNr));
            
      }
   private MessageTypes.Std.Float64 PrepareMessage(float laser)
   {
    MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
    message.data = laser;

    return message;
   }
   }
}
