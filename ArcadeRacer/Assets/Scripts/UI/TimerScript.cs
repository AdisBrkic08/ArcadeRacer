using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI Timer;
    void Update()
    {
        time += Time.deltaTime;
        Timer.text = Mathf.Floor(time).ToString();
    }
}
