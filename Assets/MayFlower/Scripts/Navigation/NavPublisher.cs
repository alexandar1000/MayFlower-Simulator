/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// Added allocation free alternatives
// UoK , 2019, Odysseas Doumas (od79@kent.ac.uk / odydoum@gmail.com)

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class NavPublisher : UnityPublisher<MessageTypes.Geometry.Pose>
    {
        //public Transform PublishedTransform;

        public string FrameId = "Unity";
        public Vector3 pos_boat;

        private MessageTypes.Geometry.Pose message;

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
            message = new MessageTypes.Geometry.Pose
            {
                //header = new MessageTypes.Std.Header()
                //{
                //    frame_id = FrameId
                //}
            };
        }

        private void UpdateMessage()
        {
            //message.header.Update();
            pos_boat.x = GameObject.Find("Boat").transform.position.x;
            pos_boat.y = GameObject.Find("Boat").transform.position.y;
            pos_boat.z = GameObject.Find("Boat").transform.position.z;

            message.position = GetGeometryPoint(pos_boat);
            //GetGeometryPoint(PublishedTransform.position.Unity2Ros(), message.pose.position);
            //GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros(), message.pose.orientation);

            Publish(message);
        }

        private MessageTypes.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            MessageTypes.Geometry.Point geometryPoint = new MessageTypes.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            
            return geometryPoint;
        }

/*
        private static void GetGeometryPoint(Vector3 position, MessageTypes.Geometry.Point geometryPoint)
        {
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
        }
*/

        private static void GetGeometryQuaternion(Quaternion quaternion, MessageTypes.Geometry.Quaternion geometryQuaternion)
        {
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
        }

    }
}
