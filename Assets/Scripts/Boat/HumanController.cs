﻿using UnityEngine;
using UnityEngine.InputSystem;
using RosSharp.RosBridgeClient;

namespace BoatAttack
{
    /// <summary>
    /// This sends input controls to the boat engine if 'Human'
    /// </summary>
    public class HumanController : BaseController
    {
        private InputControls _controls;

        public bool autoPilot = false;

        public bool unityControls = true;

        public NavSubscriber navSubscriber;

        public RemoteControlsSubscriber remoteControlsSubscriber;

        private float _throttle;
        private float _steering;

        private bool _paused;
        
        private void Awake()
        {
            
            _controls = new InputControls();
            if (!unityControls) return;
            
            _controls.BoatControls.Trottle.performed += context => _throttle = context.ReadValue<float>();
            _controls.BoatControls.Trottle.canceled += context => _throttle = 0f;
            
            _controls.BoatControls.Steering.performed += context => _steering = context.ReadValue<float>();
            _controls.BoatControls.Steering.canceled += context => _steering = 0f;

            // _controls.BoatControls.Reset.performed += ResetBoat;
            _controls.BoatControls.Freeze.performed += FreezeBoat;

            _controls.BoatControls.Time.performed += SelectTime;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!unityControls) return;
            _controls.BoatControls.Enable();
        }

        private void OnDisable()
        {
            if (!unityControls) return;
            _controls.BoatControls.Disable();
        }

        // private void ResetBoat(InputAction.CallbackContext context)
        // {
        //     controller.ResetPosition();
        // }

        private void FreezeBoat(InputAction.CallbackContext context)
        {
            _paused = !_paused;
            if(_paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void SelectTime(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Debug.Log($"changing day time, input:{value}");
            DayNightController.SelectPreset(value);
        }

        void FixedUpdate()
        {
            if(!autoPilot){
                if (!unityControls)
                {
                    engine.Accelerate((remoteControlsSubscriber.position.x == null) ? 0 : remoteControlsSubscriber.position.x);
                    engine.Turn((remoteControlsSubscriber.position.z == null) ? 0 : remoteControlsSubscriber.position.z);
                }
                else
                {
                    engine.Accelerate(_throttle);
                    engine.Turn(_steering);
                }
            }
            else{
                engine.Accelerate((float)0.5);
                engine.Turn(navSubscriber.position.z);
            }
        }
    }
}

