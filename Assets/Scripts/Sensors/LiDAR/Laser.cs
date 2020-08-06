using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser
{
    private static 
    int LaserCounter;
    private int _laserId;
    private float _laserLength = 25f;
    

    public Laser(float laserLength)
    {
        this._laserId = LaserCounter++;
        this._laserLength = laserLength;
    }

    /*
        Fires a laser with the specified starting position and direction.
    */
    public float ShootLaser(Vector3 startPosition, Vector3 direction, bool showLaser=true)
    {
        RaycastHit raycastHit;
        if(Physics.Raycast(startPosition, direction, out raycastHit, this._laserLength)) {
            GetHitPoint();
            if(showLaser) 
            {
                // Debug.DrawLine(startPosition, raycastHit.point, Color.red);
            }
            return Vector3.Distance(startPosition, raycastHit.point);
        }
        return 0f;
    }

    private void GetHitPoint()
    {
        // retrieve the position for the ROS
    }

}
