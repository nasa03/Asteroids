﻿using UnityEngine;

namespace Aster {
namespace Player {

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement: MonoBehaviour
{
    [Range(1e-2f, 1f)]
    public float CamSpeed = .1f;

    [Range(.1f, 10f)]
    public float VCamSpeedScale = 1f;

    [Range(1e-3f, 1f)]
    public float CamCircleSpeed = .025f;

    public float Eps = 1e-3f;

    public float MaxMovementSpeed = 5f;
    public float MovementAccel = .5f;
    public float StrafeAccel = .4f;

    private float dMouseX, dMouseY, dMouseZ;
    private float dMoveForward, dMoveRightward;

    private Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        dMouseX = Input.GetAxisRaw("Mouse X");
        dMouseY = Input.GetAxisRaw("Mouse Y");

        dMouseZ = Input.GetAxis("Look Rotation");

        dMoveForward = Input.GetAxisRaw("Vertical");
        dMoveRightward = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        UpdateCameraLook();
        UpdateMovement();
    }

    private void UpdateCameraLook()
    {
        var forward = transform.forward;
        var upward = transform.up;
        var rightward = transform.right;

        var newForward = forward;
        var newUpward = upward;

        if (Mathf.Abs(dMouseY) > Eps)
            newForward = Vector3.RotateTowards(newForward, upward, VCamSpeedScale * CamSpeed * dMouseY, 0);
        if (Mathf.Abs(dMouseX) > Eps)
            newForward = Vector3.RotateTowards(newForward, rightward, CamSpeed * dMouseX, 0);

        if (Mathf.Abs(dMouseZ) > Eps)
            newUpward = Vector3.RotateTowards(newUpward, rightward, CamCircleSpeed * dMouseZ, 0);

        transform.rotation = Quaternion.LookRotation(newForward, newUpward);
    }
    private void UpdateMovement()
    {
        var v = body.velocity;
        var forward = transform.forward;
        var rightward = transform.right;

        var f = forward * dMoveForward * MovementAccel + rightward * dMoveRightward * StrafeAccel;
        body.AddForce(f, ForceMode.Impulse);

        v = body.velocity;
        if (v.magnitude > MaxMovementSpeed)
        {
            v = v.normalized * MaxMovementSpeed;
            body.velocity = v;
        }
    }
}

}}
