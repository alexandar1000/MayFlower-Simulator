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
        [NonSerialized] public bool MatchComplete;


        [NonSerialized] public readonly List<float> SplitTimes = new List<float>();

        public CinemachineVirtualCamera cam;
        private float _camFovVel;
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
            //Matrix that transforms a point from local space into world space(Read Only)
           _spawnPosition = transform.localToWorldMatrix;
            Debug.Log("spawnposition"+_spawnPosition);
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
            if(coll.gameObject.CompareTag("RespawnPoint"))
            {
                ResetPosition();
            }

            if (coll.gameObject.CompareTag("WindArea"))
            {
                windZone = coll.gameObject;
                inWindZone = true;
            }

        }

        private void OnTriggerExit(Collider coll) {
            if(coll.gameObject.CompareTag("WindArea"))
            {
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
        public void ResetPosition()
        {

            engine.RB.velocity = Vector3.zero;
            engine.RB.angularVelocity = Vector3.zero;
            engine.RB.position = _spawnPosition.GetColumn(3);
            //engine.RB.rotation = resetMatrix.rotation;

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
