using GLTFast.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Speedometer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI SpeedText;
    [SerializeField] Rigidbody carRigidbody;
  

    private void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f;
        SpeedText.text = Mathf.RoundToInt(speed) + " km/h";
    }

}
