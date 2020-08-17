using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//linkt to ros http://docs.ros.org/melodic/api/sensor_msgs/html/msg/MagneticField.html

namespace RosSharp.RosBridgeClient
{
  public class CompassSensor : UnityPublisher<MessageTypes.Std.Float64>
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



private MessageTypes.Std.Float64 PrepareMessage(float compass)
{
    MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
    message.data = compass;

    return message;
}
}

}
