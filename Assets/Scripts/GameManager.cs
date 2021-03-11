using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool paused = true;
    private static Random rand = new Random();
    private HashSet<GravityBody> selected;
    public static bool HasSelected;
    public static float GbDistance;
    public GraphicRaycaster graphicRaycaster;
    public PointerEventData pointerEventData;
    public EventSystem eventSystem;
    private static GameManager instance = null;
    public static float time;
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
        selected = new HashSet<GravityBody>();
    }

    private void Start()
    {
        Pause();
    }

    // Main loop
    private void Update()
    {
        if (!paused)
        {
            time += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool bodyHit = Physics.Raycast(ray, out hitInfo, 100000f);

            // if (bodyHit)
            // {
            //     Debug.Log("gb hit");
            //     HandleSelectedObject(hitInfo);
            // }
            // else if (!IsClickButton())
            // {
            //     selected = new HashSet<GravityBody>();
            //     HasSelected = false;
            //     Debug.Log("no hit");
            // }
            // else
            // {
            //     Debug.Log("ui hit");
            // }
        }
        if (selected.Count == 2)
        {
            List<GravityBody> list = new List<GravityBody>(selected);

            GbDistance = GravityBody.DistanceBetween(list[0], list[1]);
            HasSelected = true;
        }
    }

    private bool IsClickButton()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);
        return results.Count > 0;
    }

    private void HandleSelectedObject(RaycastHit hitInfo)
    {
        if (selected.Count >= 2)
        {
            return;
        }
        GravityBody gb = hitInfo.transform.GetComponent<GravityBody>();

        selected.Add(gb);
    }

    public void Reset()
    {
        time = 0;
        RecentTimes.Reset();
        TextManager.GetInstance().UpdateTimeText();
        TextManager.GetInstance().UpdateRecentTimeText();
        Pause();
        LoadConfig loadConfig = LoadConfig.GetInstance();
        loadConfig.SetSceneToConfigPath();
    }

    public void UpdateRecentTimes()
    {
        RecentTimes.Push(time);
        TextManager.GetInstance().UpdateRecentTimeText();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        paused = true;
        LoadConfig.GetInstance().showUI(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        paused = false;
        LoadConfig.GetInstance().showUI(false);
    }

    public void TogglePause()
    {
        if (paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void LoadJson()
    {
        LoadConfig.GetInstance().OpenFile();
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}