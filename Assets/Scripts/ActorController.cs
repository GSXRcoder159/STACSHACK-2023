using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public Transform targetTransform;

    public float dSpeed = 3f;
    public float dReverseSpeed = -2f;
    public float dRotation = 100f;
    public float dFinishRadius = 1f;
    public float stoppingSpeedLimit = 15f;

    private Actor actor;
    private Vector3 targetPosition;

    void Awake() {
        actor = GetComponent<Actor>();
    }

    void Update() {
        this.targetPosition = targetTransform.position;

        float speed = 0f;
        float rotation = 0f;
        float successDistance = dFinishRadius;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > successDistance) {
            // determine where the target is relative to the actor
            float relative_pos = Vector3.Dot(transform.forward, (targetPosition - transform.position).normalized);

            if (relative_pos > 0) {
                // target is in front of the actor
                speed = dSpeed;

                if (distance < 20f && actor.GetSpeed() > 20f) {
                    speed = 0f;
                }

            }
            else {
                // target is behind the actor
                speed = dReverseSpeed;
            }

            // determine which direction to turn
            float angle = Vector3.SignedAngle(transform.forward, (targetPosition - transform.position).normalized, Vector3.up);
            if (angle > 0) {
                rotation = dRotation;
            }
            else {
                rotation = -dRotation;
            }
        }
        else {
             if (actor.GetSpeed() > stoppingSpeedLimit) {
                speed = dReverseSpeed;
            }
            else {
                speed = 0f;
                Debug.Log("Reached target");
            }
            rotation = 0f;
        }

        // set the actor's speed and rotation
        actor.SetSpeed(speed);
        actor.SetRotation(rotation);
    }
}