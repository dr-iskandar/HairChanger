﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour {

    public Image Hair;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Close()
    {
        Application.Quit();
    }

    public void chooseHair(Sprite hair)
    {
        Hair.sprite = hair; 
    }
}
