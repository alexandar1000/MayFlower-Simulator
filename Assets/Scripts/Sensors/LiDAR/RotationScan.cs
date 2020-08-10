using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScan
{
    private int _numberOfLasers;
    private bool _showLaser;
    private float _laserLength;
    private Laser _laser;
    private float _angleDifference;
    Vector3  _currDir;
    bool theFlag = true;

    public RotationScan(int numberOfLasers, float laserLength, bool showLaser=true) 
    {
        this._numberOfLasers = numberOfLasers;
        this._showLaser = showLaser;
        this._laserLength = laserLength;
        this._laser = new Laser(laserLength);
        this._angleDifference = 360f / numberOfLasers;

    }

    // Scan all the points at once instead of invoking the method really frequently
    public float[] Scan(Vector3 startPosition, Vector3 direction) 
    {
        float[] distances = new float[_numberOfLasers];
        _currDir = direction;

        for (int i = 0; i < _numberOfLasers; i++)
        {
            distances[i] = _laser.ShootLaser(startPosition, _currDir, true);
            // TODO: This Vector3.up might not work always; check it out
            Quaternion offsetAngle = Quaternion.AngleAxis(_angleDifference, Vector3.up);
            _currDir =  offsetAngle * _currDir;
        }

        return distances;
        
    }


}
