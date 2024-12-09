using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeoMessages = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient;
using RosSharp;

namespace MayflowerSimulator.Sensors.TF
{
    public class TransformLaserPublisher : UnityPublisher<GeoMessages.TransformStamped>
    {
        private GeoMessages.TransformStamped message;
        public string FrameId = "base_link";
        public string ChildId = "base_laser";
        public Vector3 translation;
        public Quaternion quaternion;


        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            message = new GeoMessages.TransformStamped();
            message.header.frame_id = FrameId;
            message.child_frame_id = ChildId;

        }
        private void UpdateMessage()
        {
            message.header.Update();
            GetTranslation(translation.Unity2Ros(), message.transform.translation);
            GetRotation(quaternion.Unity2Ros(), message.transform.rotation);

            Publish(message);

        }
        private static void GetTranslation(Vector3 position, GeoMessages.Vector3 translate)
        {
            translate.x = position.x;
            translate.y = position.y;
            translate.z = position.z;
        }


        private static void GetRotation(Quaternion quaternion, GeoMessages.Quaternion rotate)
        {
            rotate.x = quaternion.x;
            rotate.y = quaternion.y;
            rotate.z = quaternion.z;
            rotate.w = quaternion.w;
        }


    }
}
