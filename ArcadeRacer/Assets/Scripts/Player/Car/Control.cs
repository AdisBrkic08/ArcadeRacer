using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Control : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    public float torque = 1500f;
    public float steeringMax = 30f;


    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        animateWheels();

        if (Input.GetKey(KeyCode.W)) {
            for (int i = 0; i < wheels.Length; i++) {
                wheels[i].motorTorque = torque;
            }

        }

        if (Input.GetAxis("Horizontal") != 0) {
            for (int i = 0; i < wheels.Length; i++) {
                wheels[i].steerAngle = Input.GetAxis("Horizontal") * steeringMax;
            }
           
        }else
        {
            for (int i = 0; i < wheels.Length -2; i++)
            {
                wheels[i].steerAngle = 0;
            }
        }
        

        void animateWheels()
        {
            Vector3 wheelPosition = Vector3.zero;
            Quaternion wheelRotation = Quaternion.identity;

            for (int i = 0; i < 4; i++)
            {
                wheels [i].GetWorldPose(out wheelPosition, out wheelRotation);
                wheelMesh [i].transform.position = wheelPosition;
                wheelMesh[i].transform.rotation = wheelRotation;
            }
        }


    }

   
}
