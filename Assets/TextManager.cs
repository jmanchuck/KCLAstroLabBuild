using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text timeText;
    public Text distanceText;
    public Text cameraPosText;
    public Text cameraLookatText;

    private void Start()
    {
        cameraPosText.text = "Camera position:\n" + Camera.main.transform.position.ToString();
        cameraLookatText.text = "Camera Lookat:\n" + LoadConfig.CameraLookat.ToString();
    }

    public void UpdateDistanceText()
    {
        if (!GameManager.HasSelected)
        {
            distanceText.text = "Distance: -";
        }
        else
        {
            distanceText.text = "Distance: " + GameManager.GbDistance.ToString("F2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time: " + Time.time.ToString("F2");
    }
}
