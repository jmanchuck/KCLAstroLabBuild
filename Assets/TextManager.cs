using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text timeText;
    public Text distanceText;
    public Text cameraPosText;
    public Text cameraLookatText;
    private static TextManager instance = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateCameraText()
    {
        cameraPosText.text = "cameraPosition " + Camera.main.transform.position.ToString();
        cameraLookatText.text = "cameraLookat " + LoadConfig.CameraLookat.ToString();
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

    public static TextManager GetInstance()
    {
        return instance;
    }
}
