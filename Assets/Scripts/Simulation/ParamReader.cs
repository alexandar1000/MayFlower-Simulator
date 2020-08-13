using System;
using UnityEngine;
using Unity.Simulation;

namespace Simulation
{
/*
* ParamReader.cs
*
* Get AppParams values from AppParam file passed during execution from command line.
*
* Update corresponding variables in scene.
 */
    public class ParamReader : MonoBehaviour
    {
        private Unity.Simulation.Logger paramLogger;
        private Unity.Simulation.Logger cubeLogger;
        private CubeAppParam appParams;
        private static float quitAfterSeconds;
        private static float simElapsedSeconds;

        /*
         *  Instantiates copies of Cube,
         *  moves them to a new location in the scene,
         *  and creates and logs dataPoint object.
         */
        public void ReplicateCube(GameObject cube, int replicateNum)
        {
            for (int i = 0; i < replicateNum; i++)
            {
                Vector3 cubePosition = new Vector3((i + 1) * 2.0F, 0, 0);
                GameObject newCube = Instantiate(cube, cubePosition, Quaternion.identity);
                newCube.name = newCube.name + "_" + i;
                
                // Create a new data point
                ObjectPosition cubeDataPoint = new ObjectPosition(cubePosition, newCube.name);
                cubeLogger.Log(cubeDataPoint);
            }
        }

        private void Start()
        {
            // Create a specific logger for AppParams for debugging purposes
            paramLogger = new Unity.Simulation.Logger("ParamReader");
            cubeLogger = new Unity.Simulation.Logger("CubeLogger");
            simElapsedSeconds = 0;

            if (!Configuration.Instance.IsSimulationRunningInCloud())
            {
                String appParamFilename = "app_param_0.json";
                Configuration.Instance.SimulationConfig.app_param_uri =
                    String.Format("file://{0}/StreamingAssets/{1}", Application.dataPath, appParamFilename);
                Debug.Log(Configuration.Instance.SimulationConfig.app_param_uri);
            }

            appParams = Configuration.Instance.GetAppParams<CubeAppParam>();

            if (appParams != null)
            {
                Debug.Log(appParams.ToString());
                
                // Log AppParams to DataLogger
                paramLogger.Log(appParams);
                paramLogger.Flushall();
                
                // Update the screen capture interval through an app-param
                float screenCaptureInterval = Mathf.Min(Mathf.Max(0, appParams.screenCaptureInterval), 100.0f);
                GameObject.FindGameObjectsWithTag("DataCapture")[0].GetComponent<CameraGrab>()._screenCaptureInterval =
                    screenCaptureInterval;
                //Replicate cube
                ReplicateCube(GameObject.FindGameObjectsWithTag("Cube")[0], appParams.replicateCube);

                quitAfterSeconds = appParams.quitAfterSeconds;
            }
        }

        private void Update()
        { 
            // SimulationElapsedSeconds represents the aggregate frame seconds
            // Refer
            // - https://docs.unity3d.com/ScriptReference/Time-deltaTime.html for more on seconds since last frame
            // - Unity.Simulation.DXTimeLogger for examples of logtime methods
            // - https://docs.unity3d.com/ScriptReference/Time.html for time information from Unity.
            // - https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?view=netframework-4.8 (MSDN stopwatch for elapsedSeconds)
            simElapsedSeconds += Time.deltaTime;

            if (simElapsedSeconds >= quitAfterSeconds)
            {
                // Flush all Cube data point data to file before exiting
                cubeLogger.Flushall();
                Application.Quit();
            }
        }
    }
}