using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform targetTransform;

    public float dSpeed = 5f;
    public float dReverseSpeed = -2f;
    public float dRotation = 10f;
    public float dFinishRadius = 2f;
    public float speed = 0f;
    public float rotation = 0f;
    public float stoppingDistance = 30f;
    public float stoppingSpeed = 40f;
    public float reverseTurnDistance = 20f;
    public float stoppingSpeedLimit = 15f;
    public int fitness;

    private bool finished = false;

    public Actor actor;
    private Vector3 targetPosition;

    void Awake() {
        actor = GetComponent<Actor>();
    }

    void Update() {
        this.targetPosition = targetTransform.position;

        float successDistance = dFinishRadius;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > successDistance) {
            // determine where the target is relative to the actor
            float relative_pos = Vector3.Dot(transform.forward, (targetPosition - transform.position).normalized);

            if (relative_pos > 0) {
                // target is in front of the actor
                speed = dSpeed;

                if (distance < stoppingDistance && actor.GetSpeed() > stoppingSpeed) {
                    speed = dReverseSpeed;
                }
            }
            else {
                // target is behind the actor
                if (distance < reverseTurnDistance) {
                    speed = dReverseSpeed;
                }
                else {
                    speed = dReverseSpeed;
                }
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
            }
            rotation = 0f;
            finished = true;
            fitness++;
        }
        // set the actor's speed and rotation
        actor.SetSpeed(speed);
        actor.SetRotation(rotation);
    }

    public void setTarget(Transform newTarget) {
        targetTransform = newTarget;
        finished = false;
    }

    public bool isFinished() {
        return finished;
    }
}