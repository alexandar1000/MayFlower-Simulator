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
            RotationStep = RotationPerMinute / 60;
            RotationDuratuion = 1 / RotationStep;
            RotationAxis = transform.up;
            ScansPerRotation = (int) (RotationDuratuion * ScanningFrequency);
            InitialAngle = transform.forward;
            BoatDirection = transform.parent.forward;
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
            Message.height = 1; // (uint) ScansPerColumn;
            Message.fields = new SensorMessages.PointField[3];
            StdMessages::Header header = new StdMessages.Header();
            header.frame_id = FrameId;
            Message.header = header;

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
            Message.data = new byte[0];

            Vector3 startingPosition = transform.position;
            Vector3 direction = transform.forward;
            Vector3[] points = _rotationScan.Scan(startingPosition, direction);

            TransformGlobalPointsToLocal(points);

            for (int i = 0; i < points.Length; i++)
            {
                byte[] xArr = System.BitConverter.GetBytes((float) points[i].x);
                byte[] yArr = System.BitConverter.GetBytes((float) points[i].y);
                byte[] zArr = System.BitConverter.GetBytes((float) points[i].z);

                Message.data = Message.data
                    .Concat(xArr)
                    .Concat(zArr)
                    .Concat(yArr)
                    .ToArray();
            }

            Publish(Message);
        }

        protected void TransformGlobalPointsToLocal(Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.parent.InverseTransformPoint(points[i]);
            }
        }

    }
}