using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    #region Fields
    public float speed = 0f;
    public float rotation = 0f;
    public float acceleration = 5f;
    public float rotationAcceleration = 300f;

    public float breakSpeed = 100f;
    public float reverseSpeed = 30f;
    public float idleSpeed = 10f;
    public float idleRotationSpeed = 500f;

    public float maxSpeed = 3;
    public float minSpeed = -2f;
    public float maxTurnSpeed = 150f;
    

    public float forwardVal = 0f;
    public float turnVal = 0f;

    public Rigidbody actor;
    #endregion

    public float GetSpeed() {
        return speed;
    }

    public void SetSpeed(float speed) {
        this.forwardVal = speed;
    }

    public void SetRotation(float rotation) {
        this.turnVal = rotation;
    }

    // private void Awake() {
    //     actor = GetComponent<Rigidbody>();
    // }

    void Update() {
        if (forwardVal > 0) {
            // accelerate
            speed += forwardVal * acceleration * Time.deltaTime;
        }
        else {
            if (speed > 0) {
                speed += forwardVal * breakSpeed * Time.deltaTime;
            }
            else {
                speed += forwardVal * reverseSpeed * Time.deltaTime;
            }
        }

        if (forwardVal == 0) {
            if (speed > 0) {
                speed -= idleSpeed * Time.deltaTime;
            }
            else {
                speed += idleSpeed * Time.deltaTime;
            }
        }

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        actor.velocity = transform.forward * speed;

        if (speed < 0) {
            turnVal = -turnVal;
        }

        if (turnVal > 0 || turnVal < 0) {
            // turn
            if ((rotation > 0 && turnVal < 0) || (rotation < 0 && turnVal > 0)) {
                rotation = turnVal * 20f;
            }
            rotation += turnVal * rotationAcceleration * Time.deltaTime;
        }
        else {
            // not turning
            if (rotation > 0) {
                rotation -= idleRotationSpeed * Time.deltaTime;
            }
            if (rotation < 0) {
                rotation += idleRotationSpeed * Time.deltaTime;
            }
            if (Mathf.Abs(rotation) < 1f) {
                rotation = 0f;
            }
        } 

        float speedNormal = speed / maxSpeed;
        float inverseSpeedNormal = Mathf.Clamp(1 - speedNormal, .75f, 1f);

        rotation = Mathf.Clamp(rotation, -maxTurnSpeed, maxTurnSpeed);
        actor.angularVelocity = new Vector3(0, rotation * (inverseSpeedNormal * 1f) * Mathf.Deg2Rad, 0);

        if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2) {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}

