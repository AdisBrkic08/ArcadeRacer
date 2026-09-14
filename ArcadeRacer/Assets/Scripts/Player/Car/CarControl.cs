using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarControl : MonoBehaviour
{

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        WheelCollider_FL.steerAngle = m_steeringAngle;
        WheelCollider_FR.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        WheelCollider_FL.motorTorque = m_verticalInput * motorForce;
        WheelCollider_FR.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(WheelCollider_FL, WheelFrontLeft);
        UpdateWheelPose(WheelCollider_FR, WheelFrontRight);
        UpdateWheelPose(WheelCollider_RL, WheelRearLeft);
        UpdateWheelPose(WheelCollider_RR, WheelRearRight);

    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider WheelCollider_FL, WheelCollider_FR;
    public WheelCollider WheelCollider_RL, WheelCollider_RR;
    public Transform WheelFrontLeft, WheelFrontRight;
    public Transform WheelRearLeft, WheelRearRight;
    public float maxSteerAngle = 30;
    public float motorForce = 50;


}
