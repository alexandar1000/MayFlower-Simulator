using System;
using UnityEngine;
using RosSharp.RosBridgeClient;
using SensorMessages = RosSharp.RosBridgeClient.MessageTypes.Sensor;

namespace MayflowerSimulator.Sensors.Temperature
{
    public class VideoFeedPublisher : UnityPublisher<SensorMessages.CompressedImage>
    {
        public Camera videoCamera;
        private Material mat;
        private RenderTexture renderTexture;
        public int resolutionWidth = 640;
        public int resolutionHeight = 480;
        private int bytesPerPixel = 3;
        [Range(0, 100)]
        public int qualityLevel = 50;
        private byte[] rawByteData;
        private Texture2D texture2D;
        private Rect rect;

        private SensorMessages.CompressedImage message;

        protected override void Start()
        {
            base.Start();
            renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
            videoCamera.targetTexture = renderTexture;
            rawByteData = new byte[resolutionWidth * resolutionHeight * bytesPerPixel];
            texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
            rect = new Rect(0, 0, resolutionWidth, resolutionHeight);

            message = new SensorMessages.CompressedImage();
            message.header.frame_id = "VideoCamera";
            message.format = "jpeg";

            InvokeRepeating("UpdateMessage", 1f , 1f);

        }

        private void UpdateMessage()
        {
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(rect, 0, 0);
            Array.Copy(texture2D.GetRawTextureData(), rawByteData, rawByteData.Length);
            message.header.Update();
            message.data = texture2D.EncodeToJPG(qualityLevel);
            //Debug.Log("Sending image: " + message.data);
            Publish(message);
        }
    }
}