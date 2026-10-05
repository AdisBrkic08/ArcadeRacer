using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("Drift")]
    public float driftSteerAngle = 45f;
    public float driftGrip = 0.30f;
   

    private bool drifting;
    private int direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetRearWheelGrip(normalGrip);
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        float steering = maxSteerAngle;

        if (drifting)
        {
            steering = driftSteerAngle;
        }

        m_steeringAngle = steering * m_horizontalInput;

        WheelCollider_FL.steerAngle = m_steeringAngle;
        WheelCollider_FR.steerAngle = m_steeringAngle;
    }

    public void Steer(int direction, float amount)
    {
        m_steeringAngle = (driftSteerAngle * direction) * amount;

        WheelCollider_FL.steerAngle = m_steeringAngle;
        WheelCollider_FR.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        float torque = m_verticalInput * motorForce;

        if (drifting)
        {
          
        }

        WheelCollider_RL.motorTorque = torque;
        WheelCollider_RR.motorTorque = torque;

        WheelCollider_FL.motorTorque = 0f;
        WheelCollider_FR.motorTorque = 0f;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(WheelCollider_FL, WheelFrontLeft);
        UpdateWheelPose(WheelCollider_FR, WheelFrontRight);
        UpdateWheelPose(WheelCollider_RL, WheelRearLeft);
        UpdateWheelPose(WheelCollider_RR, WheelRearRight);
    }

    private void Drift()
    {
        if (Input.GetButtonDown("Jump") &&
            !drifting &&
            Mathf.Abs(m_horizontalInput) > 0.1f &&
            rb.linearVelocity.magnitude > 3f)
        {
            drifting = true;

            direction = m_horizontalInput > 0 ? 1 : -1;

            SetRearWheelGrip(driftGrip);
        }

        if (drifting)
        {
            float amount;

            if (direction == 1)
            {
                amount = Mathf.InverseLerp(-1f, 1f, m_horizontalInput) * 2f;
               
            }
            else
            {
                amount = Mathf.InverseLerp(-1f, 1f, m_horizontalInput);

                amount = (1f - amount) * 2f;
            }

            Steer(direction, amount);

            rb.AddTorque(
                Vector3.up *
                direction *
                Mathf.Abs(m_horizontalInput) *
                300f,
                ForceMode.Force
            );
        }

        if (Input.GetButtonUp("Jump") && drifting)
        {
            drifting = false;

            SetRearWheelGrip(normalGrip);
        }
    }

    private void SetRearWheelGrip(float stiffness)
    {
        WheelFrictionCurve leftFriction =
            WheelCollider_RL.sidewaysFriction;

        WheelFrictionCurve rightFriction =
            WheelCollider_RR.sidewaysFriction;

        leftFriction.stiffness = stiffness;
        rightFriction.stiffness = stiffness;

        WheelCollider_RL.sidewaysFriction = leftFriction;
        WheelCollider_RR.sidewaysFriction = rightFriction;
    }

    private void UpdateWheelPose(
        WheelCollider _collider,
        Transform _transform)
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
        Drift();
    }

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    float normalGrip = 1f;
    private Rigidbody rb;

    public WheelCollider WheelCollider_FL;
    public WheelCollider WheelCollider_FR;
    public WheelCollider WheelCollider_RL;
    public WheelCollider WheelCollider_RR;

    public Transform WheelFrontLeft;
    public Transform WheelFrontRight;
    public Transform WheelRearLeft;
    public Transform WheelRearRight;

    public float maxSteerAngle = 35f;
    public float motorForce = 1000f;
}