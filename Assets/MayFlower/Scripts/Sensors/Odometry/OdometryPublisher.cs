using UnityEngine;
using GeoMessages = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using NavMessages = RosSharp.RosBridgeClient.MessageTypes.Nav;
using RosSharp.RosBridgeClient;
using RosSharp;

namespace MayflowerSimulator.Sensors.Odometry
{
    public class OdometryPublisher : UnityPublisher<NavMessages.Odometry>
    {
        public Transform PublishedTransform;
        public string FrameId = "odom";
        public string ChildId = "base_link";

        private NavMessages.Odometry message;
        private float previousRealTime;
        private Vector3 previousPosition = Vector3.zero;
        private Quaternion previousRotation = Quaternion.identity;

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
            message = new NavMessages.Odometry();
            message.header.frame_id = FrameId;
            message.child_frame_id = ChildId;
           
        }
        private void UpdateMessage()
        {
            message.header.Update();
            double[] cov = {0.01, 0.0, 0.0, 0.0, 0.0, 0.0,
                         0.0, 0.01, 0.0, 0.0, 0.0, 0.0,
                         0.0, 0.0, 0.01, 0.0, 0.0, 0.0,
                         0.0, 0.0, 0.0, 0.0001, 0.0, 0.0,
                         0.0, 0.0, 0.0, 0.0, 0.0001, 0.0,
                         0.0, 0.0, 0.0, 0.0, 0.0, 0.0001};


            GetGeometryPoint(PublishedTransform.position.Unity2Ros(), message.pose.pose.position);
            GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros(), message.pose.pose.orientation);
            message.pose.covariance = cov;

            float deltaTime = Time.realtimeSinceStartup - previousRealTime;

            Vector3 linearVelocity = (PublishedTransform.position - previousPosition) / deltaTime;
            Vector3 angularVelocity = (PublishedTransform.rotation.eulerAngles - previousRotation.eulerAngles) / deltaTime;

            message.twist.twist.linear = GetGeometryVector3(linearVelocity.Unity2Ros()); 
            message.twist.twist.angular = GetGeometryVector3(-angularVelocity.Unity2Ros());
            message.twist.covariance = cov;

            previousRealTime = Time.realtimeSinceStartup;
            previousPosition = PublishedTransform.position;
            previousRotation = PublishedTransform.rotation;
            Publish(message);
        }

        private static GeoMessages.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            GeoMessages.Vector3 geometryVector3 = new GeoMessages.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }

        private static void GetGeometryPoint(Vector3 position, GeoMessages.Point geometryPoint)
        {
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
        }

        private static void GetGeometryQuaternion(Quaternion quaternion, GeoMessages.Quaternion geometryQuaternion)
        {
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
        }
    }
}



