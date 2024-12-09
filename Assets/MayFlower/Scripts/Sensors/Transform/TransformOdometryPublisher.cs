using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeoMessages = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient;
using RosSharp;

namespace MayflowerSimulator.Sensors.TF
{
    public class TransformOdometryPublisher : UnityPublisher<GeoMessages.TransformStamped>
    {
        public Transform PublishedTransform;
        private GeoMessages.TransformStamped message;
        public string FrameId = "odom";
        public string ChildId = "base_link";


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
            GetTranslation(PublishedTransform.position.Unity2Ros(), message.transform.translation);
            GetRotation(PublishedTransform.rotation.Unity2Ros(), message.transform.rotation);

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
