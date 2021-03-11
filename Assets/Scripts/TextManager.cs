using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text timeText;
    public Text recentTimesText;
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

    public void UpdateTimeText()
    {
        timeText.text = "Time: " + GameManager.time.ToString("F2");
    }

    public void UpdateRecentTimeText()
    {
        string s = "";
        foreach (float time in RecentTimes.GetTimes())
        {
            s += time.ToString("F2") + "\n";
        }
        recentTimesText.text = s;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeText();
    }

    public static TextManager GetInstance()
    {
        return instance;
    }
}
