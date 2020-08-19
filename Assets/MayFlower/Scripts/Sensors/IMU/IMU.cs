using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;


public class IMU : MonoBehaviour
{
    public Transform Boat;
    private int frame = 0;

    //For Linear
    private Queue<Vector3> PosQueue;
    private Queue<Vector3> VQueue;   
    public Vector3 currentV;   
    public Vector3 selfV;
    public static Vector3 Accelerate_Linear;

    //For Angular
    private Queue<Vector3> AngQueue;
    private float roll1;
    private float roll2;    
    private float pitch1;
    private float pitch2;    
    private float yaw1;
    private float yaw2; 
    public static Vector3 currentAngularVelocity; //(rad/s^2), rad = degree * Math.PI / 180


    void Start()
    {
        var pos = Boat.position;
        PosQueue = new Queue<Vector3>();
        PosQueue.Enqueue(new Vector3(pos.x, pos.y, pos.z));
        AngQueue = new Queue<Vector3>();
        AngQueue.Enqueue(new Vector3(Rotate.roll, Rotate.yaw, Rotate.pitch));

        //test
       /* UnityEngine.Debug.Log(Boat.InverseTransformDirection(new Vector3(0, 0, 1))); //(0,0,1) =>(1, 0, 0): Self.x = forward; 
        UnityEngine.Debug.Log(Boat.InverseTransformDirection(new Vector3(0, 1, 0))); //(0,1,0) =>(0, 1, 0): Self.y = up; 
        UnityEngine.Debug.Log(Boat.InverseTransformDirection(new Vector3(1, 0, 0))); //(1,0,0) =>(0, 0, -1): Self.z = left; */
    }
  
    void FixedUpdate()
    {
        frame++;
        if(frame == 10 || frame == 20 || frame == 30 || frame == 40 || frame == 50)
        {
            //Linear
            var pos = Boat.position;
            Vector3 lastP = new Vector3(pos.x, pos.y, pos.z);
            PosQueue.Enqueue(lastP);
            if (frame == 50)
            {
                currentV = (lastP - PosQueue.Peek()) / 1;
                PosQueue.Dequeue();
               
                selfV = Boat.InverseTransformDirection(currentV);
                VQueue = new Queue<Vector3>();
                VQueue.Enqueue(selfV);      

            }
        }
        else if(frame % 10 == 0)
        {            
            var pos = Boat.position;
            Vector3 last = new Vector3(pos.x, pos.y, pos.z);
            PosQueue.Enqueue(last);           
            Vector3 start = PosQueue.Dequeue();
            currentV = (last - start) / 1;
            selfV = Boat.InverseTransformDirection(currentV);
            if (frame == 60 || frame == 70 || frame == 80 || frame == 90)
            {
                VQueue.Enqueue(selfV);
            }else if(VQueue.Count>0)
            {
                VQueue.Enqueue(selfV);
                Accelerate_Linear = (selfV - VQueue.Dequeue()) / 1;
            }
        }


        //Angular
        if (frame == 10 || frame == 20 || frame == 30 || frame == 40)
        {
            AngQueue.Enqueue(new Vector3(Rotate.roll, Rotate.yaw, Rotate.pitch));

        }else if(frame % 10 == 0) {
            Vector3 first = AngQueue.Dequeue();
            roll1 = first[0];
            yaw1 = first[1];
            pitch1 = first[2];

            roll2 = Rotate.roll;
            pitch2 = Rotate.pitch;
            yaw2 = Rotate.yaw;
            AngQueue.Enqueue(new Vector3(roll2, yaw2, pitch2));

            //Check if yaw1 and yaw2 cross 0 degree 
            if(yaw2 >= 270 && yaw1 <= 90)
            {
                currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle(-(360 - yaw2 + yaw1) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
            }else if (yaw1 >= 270 && yaw2 <= 90)
            {
                currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle(-(yaw1 - 360 - yaw2) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
            }
            else
            {
                currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle((yaw2 - yaw1) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
            }            
        }
    }
}
