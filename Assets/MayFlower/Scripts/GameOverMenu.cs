﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoatAttack;
using MayflowerSimulator.Sensors.Battery;

public class GameOverMenu : MonoBehaviour
{
    public GameObject EndMenuUI;
    public Boat boat;


    // Update is called once per frame
    void Update()
    {
        // TODO: Depending on the battery solution, update this segment commented-out bellow
        // if (Battery.boatStatus == 1)
        // {
        //     EndPause();
        // }
        // else
        // {
        //     Live();
        // }

    }

    public void EndPause()
    {
        EndMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Live()
    {
        EndMenuUI.SetActive(false);
        Time.timeScale = 1f;

    }
    public void Restart()
    {
        boat.ResetPosition();

    }
    public void Quit()
    {
        Debug.Log("Quitting game");
        Application.Quit();

    }
}