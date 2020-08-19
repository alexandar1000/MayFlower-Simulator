using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Environment.Battery
{
    public class Battery : MonoBehaviour
    {
        public float BatteryChargeMinutesDuration;
        public float BatteryDischargeMinutesDuration;
        public float InitialBatteryChargePercentage;
        public float Voltage { get; } = 3f;
        public float consumeRate = 0.5f;
        public float Charge;
        protected bool IsCharging = false;
        private GameObject homeZone;

        protected const byte POWER_SUPPLY_STATUS_UNKNOWN = 0;
        protected const byte POWER_SUPPLY_STATUS_CHARGING = 1;
        protected const byte POWER_SUPPLY_STATUS_DISCHARGING = 2;
        protected const byte POWER_SUPPLY_STATUS_NOT_CHARGING = 3;
        protected const byte POWER_SUPPLY_STATUS_FULL = 4;

        // Start is called before the first frame update
        void Start()
        {
            Charge = InitialBatteryChargePercentage;
            
            float batteryChargePercentDuration = (BatteryChargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("ChargeBattery", batteryChargePercentDuration, batteryChargePercentDuration);

            float batteryDischargePercentDuration = (BatteryDischargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("DischargeBattery", batteryDischargePercentDuration, batteryDischargePercentDuration);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (IsCharging)
            {
                Charge += Time.deltaTime * consumeRate;
                Charge = Mathf.Min(100, Charge);                
            }

            else
            {
                //update boat status
                if (Charge > 0)
                {
                    Charge -= Time.deltaTime * consumeRate; //Time.time: number of seconds from the start of game       

                }
                else
                {
                    Debug.Log("The battery runs out off power, boat stopped.");
                }


            }

        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.CompareTag("HomeArea"))
            {
                homeZone = coll.gameObject;
                IsCharging = true;
            }
        }

        private void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.CompareTag("HomeArea"))
            {
                IsCharging = false;
            }

        }

        //// Update the battery state when the Battery is Discharging
        //protected void DischargeBattery()
        //{
        //    if (!IsCharging && Charge > 0f)
        //    {
        //        Charge = Mathf.Max(0, Charge-1);
        //    }
        //}


        // Get current charge returns the battery percentage normalised to the scale of 0 to 1
        public float GetCurrentCharge()
        {
            return Charge / 100f;
        }

        public byte GetCurrentChargingStatus()
        {
            if (IsCharging)
            {
                if (Charge == 100f)
                {
                    return POWER_SUPPLY_STATUS_FULL;
                }

                return POWER_SUPPLY_STATUS_CHARGING;
            } else 
            {
                if (Charge == 0f)
                {
                    return POWER_SUPPLY_STATUS_NOT_CHARGING;
                }

                return POWER_SUPPLY_STATUS_DISCHARGING;
            }
        }
    }
}