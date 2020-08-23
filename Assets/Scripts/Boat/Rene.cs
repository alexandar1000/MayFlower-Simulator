using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cinemachine;
using BoatAttack.UI;
using Object = UnityEngine.Object;
using MayflowerSimulator.Sensors.Battery;

namespace BoatAttack
{
    /// <summary>
    /// This is an overall controller for a boat
    /// </summary>
    public class Rene : Boat
    {
        
        
        private Rigidbody rb;

        private float _camFovVel;
        private Object _controller;
        private int _playerIndex;
        
        // Modified Content
        public bool inWindZone = false;
        [NonSerialized] public GameObject windZone;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();

        }

        private void Update()
        {
            if (Battery.boatStatus == 1)
            {
                ResetPosition();
            }

        }


        private void FixedUpdate() {
            if(inWindZone){
                rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength);
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

        }

    }

