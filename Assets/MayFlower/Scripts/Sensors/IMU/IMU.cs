using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

namespace MayflowerSimulator.Sensors.IMU
{

    public class IMU : MonoBehaviour
    {
        public Transform Boat;
        private int frame = 0;
        //For Linear
        //Use a queue PosQueue to store the position samples for 5 time segments 6 position points(1 segment = 10 frame = 0.2s each): 
        private Queue<Vector3> PosQueue;

        //Another Queue VelQueue to store the velocities for calculating the acceleration for 1s (5 time segments)
        private Queue<Vector3> VQueue;

        //Position change in the world
        public Vector3 currentV;

        //Decomposition world position vector to the boat's own x, y ,z direction 
        public Vector3 selfV;   //The velocity in self space: X,Y,Z = left, up, forward

        //Get Linear accelerations (accelerometer)
        public static Vector3 Accelerate_Linear; //X,Y,Z

        //For Angular
        //CURRENTLY CANNOT APPLY TO ROTATION QUICKER THAN 180 DEGREE / SECOND
        //Use queue AngQueue to maintain 6 value of roll, yaw, pitch in Vectors (every 10 frame = 0.2s each)
        //And calculate the angular velocity using the last item and the first item in the queue.
        private Queue<Vector3> AngQueue;

        private float roll1;
        private float roll2;    
        private float pitch1;
        private float pitch2;    
        private float yaw1;
        private float yaw2;    

        //current angular velocity (rad/s^2), rad = degree * Math.PI / 180
        public static Vector3 currentAngularVelocity;

        public static Quaternion orientation;

        // Start is called before the first frame update
        void Start()
        {
            var pos = Boat.position;
            PosQueue = new Queue<Vector3>();
            PosQueue.Enqueue(new Vector3(pos.x, pos.y, pos.z));
            AngQueue = new Queue<Vector3>();
            AngQueue.Enqueue(new Vector3(Rotate.roll, Rotate.yaw, Rotate.pitch));
            orientation = Quaternion.Euler(Boat.forward);
        }

        void FixedUpdate()
        {
            frame++;

            //Linear
            // Update is called once per frame(0.02 s)
            //1 second = 50 frame (for FixedUpdate)
            //First 50 frames, sample the position point every 10 frame and stores in the PosQueue;
            //From the 50th to 90th frame, average velocity for each 1 second can be calculated and stores in the VQueue;
            //From the 100th frame, the Accelerate_Linear can be calculated using the last item - first item
            //and the accelerate will be updated when the next velocity is added to VQueue which is every 1 second.
            if (frame == 10 || frame == 20 || frame == 30 || frame == 40 || frame == 50)
            {
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
            //For the first 50 frames sample the roll, yaw, pitch as Vectors and stored in AngQueue
            //Then for every 10 frames, get the new angular vector to be enqueue, and dequeue the first sample in the AngQueue to calculate the angular velocity
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

                //Check if yaw1 and yaw2 cross the degree 0
                if(yaw2 >= 270 && yaw1 <= 90)
                {
                    currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle(-(360 - yaw2 + yaw1) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
                } else if (yaw1 >= 270 && yaw2 <= 90)
                {
                    currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle(-(yaw1 - 360 - yaw2) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
                }
                else
                {
                    currentAngularVelocity = new Vector3(Convert.ToSingle((roll2 - roll1) * Math.PI / 180), Convert.ToSingle((yaw2 - yaw1) * Math.PI / 180), Convert.ToSingle((pitch2 - pitch1) * Math.PI / 180));
                }            
            }

            //Orientation
            orientation = Quaternion.Euler(Boat.forward);
        }
    }
}
