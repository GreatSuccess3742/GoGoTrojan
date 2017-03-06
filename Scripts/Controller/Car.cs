﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public const float FREEZE_DRAG = 100000;
    private Vector3 com = new Vector3(0,0,0);
    private float maxMotorTorque = 10000;
    private float maxBrakeTorque = 20000;
    private float maxSteerAngle = 15;
    public GameObject frontLeft, frontRight, rearLeft, rearRight;
    public Text speedText;
    public float topSpeed = 100 * 1000 / 3600;//(100 km/h)
    public float carVelocity;

    
    private Rigidbody rb;
    private WheelController flController, frController, rlController, rrController;
    

    private float maxTiltAngle = 20;


    //private float stuckSpeedThres = 3;

    private Skill mySkill;

    private float myHP;
    private float myMaxHP;
    private float myMaxMP=100;
    private float myMP=100;
    private float myAttackPower;
    

    private bool stopFlag = false;

    // Use this for initialization 
    void Start()
    {
        mySkill = GetComponent<Skill>();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass =com;
        if (speedText != null)
        {
            speedText.text = "0 km/h";
        }
        flController = frontLeft.GetComponent<WheelController>();
        frController = frontRight.GetComponent<WheelController>();
        rlController = rearLeft.GetComponent<WheelController>();
        rrController = rearRight.GetComponent<WheelController>();
        
    }



    // Update is called once per frame 
    void Update()
    {
        carVelocity = GetComponent<Rigidbody>().velocity.magnitude;
        if (transform.rotation.eulerAngles.x > 180)
        {
            transform.rotation = Quaternion.Euler(
                 Mathf.Clamp(transform.rotation.eulerAngles.x, 360- maxTiltAngle, 360),
                 transform.rotation.eulerAngles.y,
                 transform.rotation.eulerAngles.z
                 );
        }
        else if (transform.rotation.eulerAngles.x <180)
        {
            transform.rotation = Quaternion.Euler(
                 Mathf.Clamp(transform.rotation.eulerAngles.x, 0, maxTiltAngle),
                 transform.rotation.eulerAngles.y,
                 transform.rotation.eulerAngles.z
                 );
        }

        if (transform.rotation.eulerAngles.z > 180)
        {
            transform.rotation = Quaternion.Euler(
                 transform.rotation.eulerAngles.x,
                 transform.rotation.eulerAngles.y,
                 Mathf.Clamp(transform.rotation.eulerAngles.z, 360- maxTiltAngle, 360)
                 );
        }
        else if (transform.rotation.eulerAngles.z < 180)
        {
            transform.rotation = Quaternion.Euler(
                 transform.rotation.eulerAngles.x,
                 transform.rotation.eulerAngles.y,
                 Mathf.Clamp(transform.rotation.eulerAngles.z, 0, maxTiltAngle)
                 );
        }

        if (speedText != null)
        {
            speedText.text = Mathf.Round((rb.velocity.magnitude * 3600 / 1000) * 10) / 10f + " km/h";
        }
    }
    void FixedUpdate()
    {
        if (stopFlag)
        {
            return;
        }
                
        if (rb.velocity.magnitude > topSpeed)
        {
            float slowDownRatio = rb.velocity.magnitude / topSpeed;
            rb.velocity /= slowDownRatio;
        }
    }

    public void speedDebuff(float debuffRatio)
    {
        topSpeed /= debuffRatio;
    }

    public void removeDebuff(float debuffRatio)
    {
        topSpeed *= debuffRatio;
    }

    public void ApplyThrottle(float throttleFactor)
    {
        if (stopFlag)
        {
            throttleFactor = 0;
        }
        flController.ApplyThrottle(maxMotorTorque * throttleFactor);
        frController.ApplyThrottle(maxMotorTorque * throttleFactor);
        rlController.ApplyThrottle(maxMotorTorque * throttleFactor);
        rrController.ApplyThrottle(maxMotorTorque * throttleFactor);
    }

    public void ApplyBrake(float brakeFactor)
    {
        flController.ApplyBrake(maxBrakeTorque* brakeFactor);
        frController.ApplyBrake(maxBrakeTorque* brakeFactor);
        rlController.ApplyBrake(maxBrakeTorque* brakeFactor);
        rrController.ApplyBrake(maxBrakeTorque* brakeFactor);
    }

    public void useSkill()
    {
        mySkill.activateSkill();
        myMP = 0;
    }

   

    public void ApplyPedal(float motorTorqueFactor, float brakeTorqueFactor)
    {
        ApplyBrake(brakeTorqueFactor);
        ApplyThrottle(motorTorqueFactor);
    }

    public void ApplySteer(float steerFactor)
    {
        float steerAngle = steerFactor * maxSteerAngle;
        flController.ApplySteer(steerAngle);
        frController.ApplySteer(steerAngle);
        
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        //source.minDistance = 5;
        //source.dopplerLevel = 0;
        return source;
    }

    public void setTopSpeed(float speed)
    {
        topSpeed = speed;
    }

    public float getTopSpeed()
    {
        return topSpeed;
    }

    private void diasbleAllController()
    {

        AIScript ai = GetComponent<AIScript>();
        PlayerController player = GetComponent<PlayerController>();
        WheelController[] wheels = GetComponentsInChildren<WheelController>();
        if (ai != null)
        {
            ai.PauseAI();
        }
        if (player != null)
        {
            player.enabled = false;
        }

        foreach (WheelController wheel in wheels)
        {
            if (wheel != null)
            {
                wheel.enabled = false;
            }
        }
    }

    private void enableAllController()
    {
        AIScript ai = GetComponent<AIScript>();
        PlayerController player = GetComponent<PlayerController>();
        WheelController[] wheels = GetComponentsInChildren<WheelController>();
        if (ai != null)
        {
            ai.resumeAI();
        }
        if (player != null)
        {
            player.enabled = true;
        }

        foreach (WheelController wheel in wheels)
        {
            if (wheel != null)
            {
                wheel.enabled = true;
            }
        }
    }

    public void stopRunning()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.drag = FREEZE_DRAG;
            rb.angularDrag = FREEZE_DRAG;
        }

        diasbleAllController();

        stopFlag = true;

        mySkill.stopSkill();
    }

    public void startRunning()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.drag = 0;
            rb.angularDrag = 0.05f;
        }

        enableAllController();
        stopFlag = false;
    }

    

    public void HPInitialize(float point)
    {
        myMaxHP = point;
        myHP = point;
    }

    public float getMaxHP()
    {
        return myMaxHP;
    }

    public float getHP()
    {
        return myHP;
    }

    public void decreaseHP(float point)
    {
        myHP = Mathf.Clamp(myHP - point, 0 , myMaxHP);
    }

    public void increaseHP(float point)
    {
        myHP = Mathf.Clamp(myHP + point, 0, myMaxHP);
    }

    public void MPInitialize(float maxPoint, float point = 0)
    {
        myMaxMP = maxPoint;
        myMP = point; 
    }

    public float getMaxMP()
    {
        return myMaxMP;
    }

    public float getMP()
    {
        return myMP;
    }

    public void decreaseMP(float point)
    {
        myMP = Mathf.Clamp(myMP - point, 0, myMaxMP);
    }

    public void increaseMP(float point)
    {
        myMP = Mathf.Clamp(myMP + point, 0, myMaxMP);
    }
    public void AttackInitialize(float point)
    {
        myAttackPower = point;
    }

    public float getAttackPower()
    {
        return myAttackPower;
    }

    public void decreaseAttackPower(float point)
    {
        myAttackPower = Mathf.Max(myAttackPower - point, 0);
    }

    public void increaseAttackPower(float point)
    {
        myAttackPower += point; 
    }

  


}