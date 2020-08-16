using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using UnityEngine;

namespace MayflowerSimulator.Sensors.IMU
{

    public class Rotate : MonoBehaviour
    {
        public Transform Boat;
        Vector3 eulerAngles;
        public float Rotate_X;
        public float Rotate_Y;
        public float Rotate_Z;
        public static float roll; //roll left (<0); roll right (>0).
        public static float pitch; //bow dive (<0); bow upturned (>0).
        public static float yaw; //clockwise 0 to 360(=0)
    


        // Update is called once per frame
        void Update()
        {
            eulerAngles = Boat.rotation.eulerAngles;
            Rotate_X = Convert.ToSingle(Math.Round(eulerAngles.x, 3));
            Rotate_Y = Convert.ToSingle(Math.Round(eulerAngles.y, 3));
            Rotate_Z = Convert.ToSingle(Math.Round(eulerAngles.z, 3));
            //roll
            if(Rotate_X < 90)
            {
                roll = Rotate_X * (-1);
            }
            else if(Rotate_X > 270)
            {
                roll = 360 - Rotate_X;
            }

            //pitch
            if (Rotate_Z > 180)
            {
                pitch = -(360 - Rotate_Z);
            }else
            {
                pitch = Rotate_Z;
            }

            //yaw
            //yaw = (Rotate_Y + 360 - orientation) % 360;
            yaw = Rotate_Y;
        }
    }
}
