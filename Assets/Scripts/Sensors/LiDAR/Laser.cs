using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser
{
    private static 
    int LaserCounter;
    private int _laserId;
    private float _laserLength = 25f;
    // private Ray _ray;
    // private RaycastHit _raycastHit;

    public Laser(float laserLength)
    {
        this._laserId = LaserCounter++;
        this._laserLength = laserLength;
    }

    public void ShootLaser(Vector3 startPosition, Vector3 direction)
    {
        RaycastHit raycastHit;
        Debug.Log("Here");

        if(Physics.Raycast(startPosition, direction, out raycastHit, this._laserLength)) {
            
            Debug.DrawLine(startPosition, raycastHit.point, Color.red);
        }
    }

}
