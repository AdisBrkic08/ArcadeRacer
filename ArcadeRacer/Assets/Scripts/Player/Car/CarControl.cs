using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("Drift")]
    public float driftSteerAngle = 35f;
    public float driftGrip = 0.30f;

    private bool drifting;
    private int direction;

    [Range(0, 1)][SerializeField] private float m_TractionControl;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRearWheelGrip(normalGrip);
    }

    private void Update()
    {
        GetInput();
        UpdateWheelPoses();

        if (Input.GetButtonDown("Jump") &&
            !drifting &&
            Mathf.Abs(m_horizontalInput) > 0.1f &&
            rb.linearVelocity.magnitude > 3f)
        {
            drifting = true;
            direction = m_horizontalInput > 0 ? 1 : -1;
            SetRearWheelGrip(driftGrip);
        }

        if (Input.GetButtonUp("Jump") && drifting)
        {
            drifting = false;
            SetRearWheelGrip(normalGrip);

            Vector3 localAng = transform.InverseTransformDirection(rb.angularVelocity);
            localAng.y *= 0.2f;
            rb.angularVelocity = transform.TransformDirection(localAng);
        }
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
        if (drifting)
        {
            float amount = (direction == 1)
                ? Mathf.Lerp(0.5f, 1f, Mathf.InverseLerp(-1f, 1f, m_horizontalInput))
                : Mathf.Lerp(0.5f, 1f, Mathf.InverseLerp(1f, -1f, m_horizontalInput));

            Steer(direction, amount);

            rb.AddTorque(Vector3.up * direction * Mathf.Abs(m_horizontalInput) * 100f, ForceMode.Force);
        }
        else
        {
            Vector3 localAng = transform.InverseTransformDirection(rb.angularVelocity);
            localAng.y = Mathf.Clamp(localAng.y, -2f, 2f);
            rb.angularVelocity = transform.TransformDirection(localAng);
        }
    }

    private void SetRearWheelGrip(float stiffness)
    {
        WheelFrictionCurve leftFriction = WheelCollider_RL.sidewaysFriction;
        WheelFrictionCurve rightFriction = WheelCollider_RR.sidewaysFriction;

        leftFriction.stiffness = stiffness;
        rightFriction.stiffness = stiffness;

        WheelCollider_RL.sidewaysFriction = leftFriction;
        WheelCollider_RR.sidewaysFriction = rightFriction;
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos;
        Quaternion _quat;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
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

    public float maxSteerAngle = 15f;
    public float motorForce = 750f;
}