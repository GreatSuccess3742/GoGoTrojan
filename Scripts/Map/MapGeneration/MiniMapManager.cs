﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour {
    //private miniMapMark[] markList;
    //private int playerMarkIdx;
    private Camera minimapCam;
    // Use this for initialization
    void OnDrawGizmos () {
        minimapCam = GetComponentInChildren<Camera>();
        //markList = GetComponentsInChildren<miniMapMark>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        minimapCam = GetComponentInChildren<Camera>();

    }

    public void setCamTarget(Transform player)
    {
        minimapCam = GetComponentInChildren<Camera>();
        //markList = GetComponentsInChildren<miniMapMark>();
        minimapCam.gameObject.GetComponent<MiniMapCameraFollow>().setTarget(player);
    }
    
}
