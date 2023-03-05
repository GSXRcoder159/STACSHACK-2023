using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public Transform targetTransform;

    public float dSpeed = 3f;
    public float dReverseSpeed = -2f;
    public float dRotation = 100f;
    public float dFinishRadius = 1f;
    public float stoppingDistance = 30f;
    public float stoppingSpeed = 40f;
    public float reverseTurnDistance = 20f;
    public float stoppingSpeedLimit = 50f;

    private bool finished = false;

    public Actor actor;
    private Vector3 targetPosition;

    void Start() {
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
                speed = 1f;

                if (distance < stoppingDistance && actor.GetSpeed() > stoppingSpeed) {
                    speed = -1f;
                }

            }
            else {
                // target is behind the actor
                if (distance < reverseTurnDistance) {
                    speed = -1f;
                }
                else {
                    speed = 1f;
                }
            }

            // determine which direction to turn
            float angle = Vector3.SignedAngle(transform.forward, (targetPosition - transform.position).normalized, Vector3.up);
            if (angle > 0) {
                rotation = 1f;
            }
            else {
                rotation = -1f;
            }
        }
        else {
            speed = 0f;
            finished = true;
            rotation = 0f;
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