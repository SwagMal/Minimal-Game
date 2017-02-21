﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {
    public GameObject player;
    public SurivalTime time;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 50), "Score: " + player.GetComponent<PlayerController>().health);
        GUI.Label(new Rect(Screen.width/2-75, 10, 150, 50), "Time alive: " + time.getTimeSurvived().ToString("0.0"));
    }
}
