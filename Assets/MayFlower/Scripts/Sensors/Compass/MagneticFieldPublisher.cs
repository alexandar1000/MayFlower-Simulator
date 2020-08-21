using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosMessages = RosSharp.RosBridgeClient.MessageTypes;
using RosSharp.RosBridgeClient;
using UnityEngine;
using System;
using RosSharp;

namespace MayflowerSimulator.Sensors.Compass
{
    public class MagneticFieldPublisher : UnityPublisher<SensorMessages::MagneticField>
    {
        public Compass compass;
        public string FrameId = "Unity";
        private SensorMessages.MagneticField message;

        protected override void Start()
        {
            base.Start();
            InitialiseMessage();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitialiseMessage()
        {
<<<<<<< HEAD
=======

>>>>>>> asj_incorporateBoatAttack
            message = new SensorMessages::MagneticField();
            message.header.frame_id = FrameId;
            message.magnetic_field = new RosMessages.Geometry.Vector3();
            message.magnetic_field_covariance = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
<<<<<<< HEAD
=======

>>>>>>> asj_incorporateBoatAttack
        }


        private void UpdateMessage()
        {
            message.header.Update();
            Vector3 heading = compass.currentRotation;
            message.magnetic_field = GetGeometryVector3(heading.Unity2Ros());
            Publish(message);
        }
<<<<<<< HEAD
=======

>>>>>>> asj_incorporateBoatAttack
        private static RosMessages.Geometry.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            RosMessages.Geometry.Vector3 geometryVector3 = new RosMessages.Geometry.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> asj_incorporateBoatAttack
