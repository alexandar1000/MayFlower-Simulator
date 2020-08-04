using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private static 
    int LaserCounter;
    // private int _laserId;
    public float LaserLength = 25f;
    // private Ray _ray;
    // private RaycastHit _raycastHit;
    // private Vector3 _startingPosition;
    // private Vector3 _direction;

    private void Update() {
        ShootLaser();
    }

    public void ShootLaser()
    {
        // this._laserId = LaserCounter++;
        // this._laserLength = laserLength;
        // this._startingPosition = transform.position;
            // private float _laserLength;
        RaycastHit raycastHit;
        Vector3 startingPosition = transform.position;
        // Vector3 _direction;
        

        if(Physics.Raycast(startingPosition, transform.forward, out raycastHit, this.LaserLength)) {

        }
        Debug.DrawLine(startingPosition, raycastHit.point);
    }

}
