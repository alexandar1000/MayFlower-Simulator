using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
//using UnityEngine;

public class IMU : MonoBehaviour
{
    //Calculate the Linear(Position) accelerations Lx(use transform.right), Ly(transform.up), Lz(transform.forward) and Angular accelerations Ax (roll), Ay (yaw), Az (pitch). 
    //Should be related to the boat's local coordinate. 
    //For Linear: decomposition world position vector to the boat's own x, y ,z direction
    //1. calculate v1 w.r.t. World Space (Use 1 frame = 0.2 s to calculate v1)
    //2. the calculating frequency will set to 0.5 second, after 0.5 seconds, calculate v2 in World Space. (1 frame = 0.2s)  
    //3. in the World Space, calculate the difference of v2 and v1 which is deltaV, and decompose deltaV to x, y, z directions (w.r.t. current Self World direction)
    //4. the average accelerations in this 0.5 second can be calculated by dividing by 0.5 for each direction.


    //New Method Start From Here!
    //Use a queue PosQueue to store the position samples for 5 time segments 6 position points(1 segment = 10 frame = 0.2s each): 
    private Queue<Vector3> PosQueue;

    //Another Queue VelQueue to store the velocities for calculating the acceleration for 1s (5 time segments)
    private Queue<Vector3> VQueue;


    GameObject boat;
    private int frame = 0;

    //Position change in the world
    public Vector3 currentV;
    public Vector3 selfV;
 
    //Get Linear accelerations (accelerometer)
    public Vector3 Accelerate_Linear;


    //Angle for roll, yaw and pitch
    private float roll1;
    private float roll2;
    private float pitch1;
    private float pitch2;
    private float yaw1;
    private float yaw2;
    //Get Angular accelerations (gyroscope)
    public Vector3 Accelerate_Angular; //deltaAngle / deltaTime ^ 2

 
    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
        //startPos = boat.transform.position;
        var pos = boat.transform.position;
        PosQueue = new Queue<Vector3>();
        PosQueue.Enqueue(new Vector3(pos.x, pos.y, pos.z));
        roll1 = Rotate.roll;
        pitch1 = Rotate.pitch;
        yaw1 = Rotate.yaw;
    }

    // Update is called once per frame (0.02 s)
    //1 second = 50 frame (for FixedUpdate)
    void FixedUpdate()
    {
        frame++;
        if(frame == 10 || frame == 20 || frame == 30 || frame == 40 || frame == 50)
        {
            var pos = boat.transform.position;
            Vector3 lastP = new Vector3(pos.x, pos.y, pos.z);
            PosQueue.Enqueue(lastP);
            if (frame == 50)
            {
                currentV = (lastP - PosQueue.Peek()) / 1;
                PosQueue.Dequeue();

                //First velocity
                selfV = boat.transform.InverseTransformDirection(currentV);
                VQueue = new Queue<Vector3>();
                VQueue.Enqueue(selfV);
            }
        }
        else if(frame % 10 == 0)
        {            
            var pos = boat.transform.position;
            Vector3 last = new Vector3(pos.x, pos.y, pos.z);
            PosQueue.Enqueue(last);           
            Vector3 start = PosQueue.Dequeue();
            currentV = (last - start) / 1;
            selfV = boat.transform.InverseTransformDirection(currentV);

            if (frame == 60 || frame == 70 || frame == 80 || frame == 90)
            {
                VQueue.Enqueue(selfV);
            }else if(VQueue.Count>0)
            {
                Accelerate_Linear = (selfV - VQueue.Dequeue()) / 1;
            }
        }

        



        //For accelerometer
        /*if(frame % 51 == 5)
        {
            currentPos = boat.transform.position;
            v1 = (currentPos - startPos) / 0.1f;
        }
        else if(frame % 51 == 46)
        {
            startPos = boat.transform.position;
        }else if(frame % 51 == 0)
        {
            currentPos = boat.transform.position;
            v2 = (currentPos - startPos) / 0.1f;
            Vector3 deltaV = v2 - v1;
            UnityEngine.Debug.Log ("deltaV: " + deltaV);
            Vector3 deltaBoatV = Decomposition(deltaV);
            Accelerate_Linear = deltaBoatV / 1f;
           
            //get new data
            startPos = currentPos;
        }*/


        //For gyroscope, 1s = 50 frame
        if (frame % 50 == 0)
        {
            roll2 = Rotate.roll;
            pitch2 = Rotate.pitch;
            yaw2 = Rotate.yaw;
            Accelerate_Angular = new Vector3(roll2 - roll1, yaw2 - yaw1, pitch2 - pitch1);
            roll1 = roll2;
            pitch1 = pitch2;
            yaw1 = yaw2;
        }
    }



    Vector3 Decomposition (Vector3 deltaV)
    {
        //UnityEngine.Debug.Log(deltaPos);
        //cosTheta = v1 Dot v2 / |v1|*|v2|;
        //|v2| = v1 * cosTheta
        Vector3 forward = boat.transform.forward.normalized; //|v2| = 1
        Vector3 right = boat.transform.right.normalized;
        Vector3 up = boat.transform.up.normalized;
        float deltaX = deltaV.magnitude * (Vector3.Dot(deltaV, right)) / deltaV.magnitude;
        float deltaY = deltaV.magnitude * (Vector3.Dot(deltaV, up)) / deltaV.magnitude;
        float deltaZ = deltaV.magnitude * (Vector3.Dot(deltaV, forward)) / deltaV.magnitude;
        return new Vector3(deltaX, deltaY, deltaZ);
    }
}
