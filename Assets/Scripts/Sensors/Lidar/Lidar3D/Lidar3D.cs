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
        public int LaserLength;
        public float RotationPerMinute;
        public int ScanningFrequency;
        public int ScansPerColumn;
        public float UpperAngleBound;
        public float LowerAngleBound;
        public string FrameId = "Unity";
        protected float RotationStep;
        protected float RotationDuratuion;
        protected Vector3 RotationAxis;
        protected int ScansPerRotation;
        public bool ShowLasers = true;
        protected Vector3 BoatDirection;
        protected Vector3 InitialAngle;
        private RotationScan3D _rotationScan;
        protected SensorMessages.PointCloud2 Message;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            _rotationScan = new RotationScan3D(ScansPerRotation, ScansPerColumn, LaserLength, UpperAngleBound, LowerAngleBound, ShowLasers);
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        void Update()
        {
            transform.Rotate(0, RotationStep * 360 * Time.deltaTime, 0);
            BoatDirection = transform.parent.forward;
        }

        protected void InitialiseMessage()
        {
            Message = new SensorMessages::PointCloud2();
            Message.width = (uint) ScansPerRotation;
            Message.height = (uint) ScansPerColumn;
            Message.fields = new SensorMessages.PointField[] {};
            StdMessages::Header header = new StdMessages.Header();
            header.frame_id = FrameId;
            SensorMessages::PointField coords = new SensorMessages.PointField(
                "x",
                0,
                SensorMessages::PointField.FLOAT64,
                1
            );

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

            Message.is_bigendian = false;
            int pointFieldsSize = pfSizes.Sum();
            Message.point_step = (uint) pointFieldsSize;
            Message.row_step = (uint) pointFieldsSize * (uint) ScansPerRotation;
            Message.is_dense = false;
        }

        protected void UpdateMessage()
        {
            Message.header.Update();
            // TODO: Add data to the message

        }

    }
}