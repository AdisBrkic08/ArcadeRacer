using Unity.ProjectAuditor.Editor;
using UnityEngine;

public class cameraController: MonoBehaviour
{
    public GameObject atatchedVehicle;
    public GameObject CameraFolder;
    public Transform[] camLocations;
    public int locationIndicator = 2;

    public Control controllerRef;

    [Range(0,1)] public float smoothTime = .5f;

    private void Start()
    {
        atatchedVehicle = GameObject.FindGameObjectWithTag("Player");
        CameraFolder = atatchedVehicle.transform.Find("CAMERA").gameObject;
        camLocations = CameraFolder.GetComponentsInChildren<Transform>();

        controllerRef = atatchedVehicle.GetComponent<Control>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (locationIndicator >= 4 || locationIndicator < 2) locationIndicator = 2;
            else locationIndicator ++;
        }

        transform.position = camLocations[locationIndicator].position * (1 - smoothTime) + transform.position * smoothTime;
        transform.LookAt(camLocations[1].transform);

        //smoothTime = (controllerRef.KPH >= 150) ? Mathf.Abs((controllerRef.KPH / 150) - 0.85f) : 0.45f;
    }
}
