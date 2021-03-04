using UnityEngine;
using System.IO;
using System;
using SimpleJSON;
using SFB;
public class LoadConfig : MonoBehaviour
{
    private JSONNode config;
    public static Vector3 CameraLookat;
    public void SetSceneToConfig()
    {
        DeleteAllGravityBody();

        // Create scene objects
        foreach (JSONNode node in config["bodies"])
        {
            CreateGravityBody(node);
        }

        GravityBody.Initialize();

        Vector3 cameraLookat = JSONNodeToVector3(config["cameraLookat"]);
        Vector3 cameraPosition = JSONNodeToVector3(config["cameraPosition"]);
        CameraLookat = cameraLookat;

        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.LookAt(cameraLookat);
    }

    public void SetGravConst()
    {
        if (config == null)
        {
            config = GetJsonNode("asdf");
        }
    }

    Vector3 JSONNodeToVector3(JSONNode node)
    {
        return new Vector3(node[0], node[1], node[2]);
    }

    private void DeleteAllGravityBody()
    {
        GravityBody.CollectAllGB();
        foreach (GravityBody gb in GravityBody.AllBodies)
        {
            DestroyImmediate(gb.gameObject);
        }
    }

    private void CreateGravityBody(JSONNode node)
    {
        string name = node["name"].Value;
        Vector3 position = new Vector3(node["position"][0], node["position"][1], node["position"][2]);
        Vector3 initVelocity = new Vector3(node["initVelocity"][0], node["initVelocity"][1], node["initVelocity"][2]);
        float mass = node["mass"].AsFloat;
        float size = node["size"].AsFloat;
        Color color = JSONNodeToColor(node["color"]);
        bool includeTrail = node["trail"].AsBool;

        if (color == null)
        {
            Debug.Log("Color is null");
            color = Color.gray;
        }

        GameObject gObj = GravityBody.CreateGravityBody(name, position, mass, size, color, includeTrail);
    }

    private Color JSONNodeToColor(JSONNode node)
    {
        float r = node[0] / 255f;
        float g = node[1] / 255f;
        float b = node[2] / 255f;
        return new Color(r, g, b, 1f);
    }

    public void OpenFile()
    {
        String[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        config = GetJsonNode(paths[0]);
        if (config == null)
        {
            return;
        }
        SetSceneToConfig();
    }

    private JSONNode GetJsonNode(String filePath)
    {
        if (File.Exists(filePath))
        {
            return JSON.Parse(File.ReadAllText(filePath));
        }
        else
        {
            Debug.Log(String.Format("Couldn't find filepath {0}", filePath));
            return null;
        }
    }
}