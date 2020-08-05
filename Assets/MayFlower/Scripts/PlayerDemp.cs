using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemp : MonoBehaviour
{
 public int maxPower = 100;
 public int currentPower;
public Battery battery;

 void Start() {
     currentPower = maxPower;
     battery.setPower(currentPower);
     
 }
 private void Update() {
     
     currentPower -= (int)Time.deltaTime;
     battery.setPower(currentPower);

     
 }
}
