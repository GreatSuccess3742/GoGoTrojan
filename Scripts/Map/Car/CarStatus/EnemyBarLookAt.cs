﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarLookAt : MonoBehaviour {

	public Transform mainCameraTrans;

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(transform.position + mainCameraTrans.rotation * Vector3.forward,
            mainCameraTrans.rotation * Vector3.up);
	}
}
