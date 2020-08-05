using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cinemachine;
using BoatAttack.UI;
using Object = UnityEngine.Object;

namespace BoatAttack
{
    /// <summary>
    /// This is an overall controller for a boat
    /// </summary>
    public class Boat : MonoBehaviour
    {
        // Boat stats
        public Renderer boatRenderer; // The renderer for the boat mesh
        public Renderer engineRenderer; // The renderer for the boat mesh
        public Engine engine;
        private Matrix4x4 _spawnPosition;
        
        private Rigidbody rb;

        // RaceStats
        [NonSerialized] public int Place = 0;
        [NonSerialized] public float LapPercentage;
        [NonSerialized] public int LapCount;
        [NonSerialized] public bool MatchComplete;
        private int _wpCount = -1;
        private WaypointGroup.Waypoint _lastCheckpoint;
        private WaypointGroup.Waypoint _nextCheckpoint;

        [NonSerialized] public readonly List<float> SplitTimes = new List<float>();

        public CinemachineVirtualCamera cam;
        private float _camFovVel;
        [NonSerialized] public RaceUI RaceUi;
        private Object _controller;
        private int _playerIndex;
        
        // Modified Content
        // Wind
        public bool inWindZone = false;
        public GameObject windZone;
        // Shader Props
        private static readonly int LiveryPrimary = Shader.PropertyToID("_Color1");
        private static readonly int LiveryTrim = Shader.PropertyToID("_Color2");

        
        private void Awake()
		{
            _spawnPosition = transform.localToWorldMatrix;
            TryGetComponent(out engine.RB);
        }

        public void Setup(int player = 1, bool isHuman = true, BoatLivery livery = new BoatLivery())
        {
            _playerIndex = player - 1;
            cam.gameObject.layer = LayerMask.NameToLayer("Player" + player); // assign player layer
            SetupController(isHuman); // create or change controller
            Colorize(livery);
        }

        void SetupController(bool isHuman)
        {
            var controllerType = typeof(HumanController);
            // If controller exists then make sure it's teh right one, if not add it
            if (_controller)
            {
                if (_controller.GetType() == controllerType) return;
                Destroy(_controller);
                _controller = gameObject.AddComponent(controllerType);
            }
            else
            {
                _controller = gameObject.AddComponent(controllerType);
            }
        }
        void Start() { 
            rb = GetComponent<Rigidbody >();

        }

        private void Update()
        {
            // UpdateLaps();

            if (RaceUi)
            {
                RaceUi.UpdatePlaceCounter(Place);
                RaceUi.UpdateSpeed(engine.VelocityMag);
            }
            
        }

        private void FixedUpdate() {
            if(inWindZone){
                rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength);
            }
            
        }


        private void LateUpdate()
        {
            if (cam)
            {
                var fov = Mathf.SmoothStep(80f, 100f, engine.VelocityMag * 0.005f);
                cam.m_Lens.FieldOfView = Mathf.SmoothDamp(cam.m_Lens.FieldOfView, fov, ref _camFovVel, 0.5f);
            }
        }

        private void OnTriggerEnter(Collider coll)
        {
            if(coll.gameObject.tag == "WindArea"){
                windZone = coll.gameObject;
                inWindZone = true;
            }
        }

        private void OnTriggerExit(Collider coll) {
            if(coll.gameObject.tag == "WindArea"){
                inWindZone = false;
            }
            
        }

        [ContextMenu("Randomize")]
        private void ColorizeInvoke()
        {
            Colorize(Color.black, Color.black, true);
        }

        private void Colorize(Color primaryColor, Color trimColor, bool random = false)
        {
            var livery = new BoatLivery
            {
                primaryColor = random ? ConstantData.GetRandomPaletteColor : primaryColor,
                trimColor = random ? ConstantData.GetRandomPaletteColor : trimColor
            };
            Colorize(livery);
        }

        /// <summary>
        /// This sets both the primary and secondary colour and assigns via a MPB
        /// </summary>
        private void Colorize(BoatLivery livery)
        {
            boatRenderer?.material?.SetColor(LiveryPrimary, livery.primaryColor);
            engineRenderer?.material?.SetColor(LiveryPrimary, livery.primaryColor);
            boatRenderer?.material?.SetColor(LiveryTrim, livery.trimColor);
            engineRenderer?.material?.SetColor(LiveryTrim, livery.trimColor);
        }

    }

    [Serializable]
    public class BoatData
    {
        public string boatName;
        public AssetReference boatPrefab;
        public BoatLivery livery;
        public bool human;
        [NonSerialized] public Boat Boat;
        [NonSerialized] public GameObject BoatObject;

        public void SetController(GameObject boat, Boat controller)
        {
            BoatObject = boat;
            this.Boat = controller;
        }
    }

    [Serializable]
    public struct BoatLivery
    {
        [ColorUsage(false)] public Color primaryColor;
        [ColorUsage(false)] public Color trimColor;
    }
}
