/* 
Implementation of the 3D Lidar which is to be attached to an object above the main vessel
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using StdMessages = RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Lidar.Lidar3D
{
    public class Lidar3D : UnityPublisher<SensorMessages::PointCloud2>
    
    {
        public float RotationsPerMinute;
        public int ScanningFrequency;
        public int ScansPerColumn;
        public float UpperAngleBound;
        public float LowerAngleBound;
        public int LaserLength;
        public string FrameId = "Unity";
        public bool ShowLasers;
        protected float RotationsPerSecond;
        protected float RotationDuratuion;
        protected Vector3 RotationAxis;
        protected int ScansPerRotation;
        protected Vector3 BoatDirection;
        protected RotationScan3D RotationScan;
        protected SensorMessages.PointCloud2 Message;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Inititalise class needed constants
            RotationsPerSecond = RotationsPerMinute / 60;
            RotationDuratuion = 1 / RotationsPerSecond;
            RotationAxis = transform.up;
            ScansPerRotation = (int) (RotationDuratuion * ScanningFrequency);
            
            // Initialise the Lidar's rotating scanner
            RotationScan = new RotationScan3D(ScansPerRotation, ScansPerColumn, LaserLength, UpperAngleBound, LowerAngleBound, ShowLasers);

            // Initialise the message and set the update message method to be called on an interval equal to the duration of the rotation
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, RotationDuratuion);
        }

        void Update()
        {
            // Animate the rotation of the lidar and  update the direction it is facing
            transform.Rotate(0, RotationsPerSecond * 360 * Time.deltaTime, 0);
            BoatDirection = transform.parent.forward;
        }

        /* 
        Initialise the PointCloud2 message initially
         */
        protected void InitialiseMessage()
        {
            Message = new SensorMessages::PointCloud2();
            Message.header.frame_id = FrameId;

            Message.fields = new SensorMessages.PointField[3];

            // PointFields attributes in order
            string[] pfNames = new string[3] {
                "x",
                "y",
                "z"
            };
            uint[] pfOffsets = new uint[] {
                0,
                4,
                8
            };
            int[] pfSizes = new int[] {
                4,
                4,
                4
            };
            byte[] pfDataTypes = new byte[] {
                SensorMessages.PointField.FLOAT32, 
                SensorMessages.PointField.FLOAT32, 
                SensorMessages.PointField.FLOAT32
            };

            // Initialise PointFields and add them to the array in the message
            for (int i = 0; i < 3; i++)
            {
                Message.fields[i] = new SensorMessages.PointField(
                    pfNames[i],
                    pfOffsets[i],
                    pfDataTypes[i],
                    1
                );
            }

            // Update the remaining message fields as per the PointCloud2 documentation
            Message.is_bigendian = false;
            int sizeOfAllPointFields = pfSizes.Sum();
            Message.width = (uint) ScansPerRotation * (uint) ScansPerColumn;
            Message.height = 1;
            Message.point_step = (uint) sizeOfAllPointFields;
            Message.row_step = (uint) sizeOfAllPointFields * (uint) ScansPerRotation * (uint) ScansPerColumn;
            Message.is_dense = false;
        }


        /* 
        Update the PointCloud2 message with the points scanned by the RotationScanner3D before sending it to ROS
        */
        protected void UpdateMessage()
        {
            // Update the two changing elements - header and data
            Message.header.Update();
            Message.data = new byte[0];

            // Scan the environment
            Vector3[ , ] points = RotationScan.Scan(transform);

            // Pack all the points into an byte array
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    byte[] xArr = System.BitConverter.GetBytes((float) points[i,j].x);
                    byte[] yArr = System.BitConverter.GetBytes((float) points[i,j].y);
                    byte[] zArr = System.BitConverter.GetBytes((float) points[i,j].z);

                    // TODO: Make a more efficient way of concatenating these
                    Message.data = Message.data
                        .Concat(xArr)
                        .Concat(zArr)
                        .Concat(yArr)
                        .ToArray();
                }
            }

            // Publish the message to ROS
            Publish(Message);
        }



    }
}