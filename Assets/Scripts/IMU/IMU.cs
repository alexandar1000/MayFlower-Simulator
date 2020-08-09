using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using UnityEngine;


public class IMU : MonoBehaviour
{
    //Linear(Position) accelerations Lx(use transform.right), Ly(transform.up), Lz(transform.forward)
    //Angular accelerations Ax (roll), Ay (yaw), Az (pitch). 
    //Should be related to the boat's local coordinate. 

   

    GameObject boat;
    private int frame = 0;
    //For Linear
    //Use a queue PosQueue to store the position samples for 5 time segments 6 position points(1 segment = 10 frame = 0.2s each): 
    private Queue<Vector3> PosQueue;

    //Another Queue VelQueue to store the velocities for calculating the acceleration for 1s (5 time segments)
    private Queue<Vector3> VQueue;

    //Position change in the world
    public Vector3 currentV;

    //Decomposition world position vector to the boat's own x, y ,z direction
    public Vector3 selfV;   //The velocity in self space
 
    //Get Linear accelerations (accelerometer)
    public Vector3 Accelerate_Linear;


    //For Angular: calculate the changing rate of roll, pitch and yaw every 1 second
    //Angle for roll, yaw and pitch
    //frame 0 ~ 50 => rollV1; frame 50 ~ 100 => rollV2; (rollV2 - rollV1) / 1 => angular acceleration roll
    private float roll1;
    private float roll2;
    private float rollV1;
    private float pitch1;
    private float pitch2;
    private float pitchV1;
    private float yaw1;
    private float yaw2;
    private float yawV1;
    private Vector3 angularV1;
    //current angular velocity
    public Vector3 currentAngularVelocity;
    //Get Angular accelerations (gyroscope)
    public Vector3 Accelerate_Angular; //deltaAngle / deltaTime (roll, yaw, pitch)

 
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
    //First 50 frames, sample the position point every 10 frame and stores in the PosQueue;
    //From the 50th to 90th frame, average velocity for each 1 second can be calculated and stores in the VQueue;
    //From the 100 th frame, the velocity changing rate (Accelerate_Linear) can be calculated using the last item in VQueue - first item in VQueue
    //and keep update the accelerate when the next velocity is added to VQueue which is every 1 second.
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
           // UnityEngine.Debug.Log(VQueue.Count);
            if (frame == 60 || frame == 70 || frame == 80 || frame == 90)
            {
                VQueue.Enqueue(selfV);
            }else if(VQueue.Count>0)
            {
                VQueue.Enqueue(selfV);
                Accelerate_Linear = (selfV - VQueue.Dequeue()) / 1;
            }
        }

        



       //For gyroscope (Angular_acceleration) 1s = 50 frame
       //angularV1 = estimated velocity at the 25th frame 
       //angularV2 = estimated velocity at the 75th frame
        if (frame % 100 == 50)
        {
            roll2 = Rotate.roll;
            pitch2 = Rotate.pitch;
            yaw2 = Rotate.yaw;
            rollV1 = roll2 - roll1;
            yawV1 = yaw2 - yaw1;
            pitchV1 = pitch2 - pitch1;
            roll1 = roll2;
            pitch1 = pitch2;
            yaw1 = yaw2;
            angularV1 = new Vector3(rollV1, yawV1, pitchV1);
            currentAngularVelocity = angularV1;
        }else if (frame % 100 == 0)
        {
            roll2 = Rotate.roll;
            pitch2 = Rotate.pitch;
            yaw2 = Rotate.yaw;
            currentAngularVelocity = new Vector3(roll2-roll1, yaw2-yaw1, pitch2-pitch1);
            Accelerate_Angular = currentAngularVelocity - angularV1; //deltaV / 1(s)
            roll1 = roll2;
            pitch1 = pitch2;
            yaw1 = yaw2;
        }
    }


    //Same function as transform.InverseTransformDirection() does.
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
