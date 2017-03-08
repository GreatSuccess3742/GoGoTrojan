﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {
    void OnDrawGizmos()
    {
        carCheckPoint[] points = GetComponentsInChildren<carCheckPoint>();
        for (int i = 0; i < points.Length; ++i)
        {
            points[i].dist = i;
        }


    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
