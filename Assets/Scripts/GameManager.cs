using UnityEngine;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private TextManager textManager;
    public static bool paused = true;
    private static Random rand = new Random();
    private HashSet<GravityBody> selected;
    public static bool HasSelected;
    public static float GbDistance;
    public GraphicRaycaster graphicRaycaster;
    public PointerEventData pointerEventData;
    public EventSystem eventSystem;
    private void Awake()
    {
        selected = new HashSet<GravityBody>();
        textManager = GetComponent<TextManager>();
        Pause();
    }

    // Main loop
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                HandleSelectedObject(hitInfo);
            }
            else if (!IsClickButton())
            {
                selected = new HashSet<GravityBody>();
                HasSelected = false;
            }
        }
        if (selected.Count == 2)
        {
            List<GravityBody> list = new List<GravityBody>(selected);

            GbDistance = Vector3.Distance(list[0].transform.position, list[1].transform.position);
            HasSelected = true;
        }
        textManager.UpdateDistanceText();
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

    public static void Pause()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public static void TogglePause()
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

    public static string RandString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            sb.Append(Random.Range(-50f, 50f).ToString());
        }
        return sb.ToString();
    }
}