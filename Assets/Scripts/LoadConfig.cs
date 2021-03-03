using UnityEngine;
using System.IO;
using System;
using SimpleJSON;

public class LoadConfig : MonoBehaviour
{
    public string fileName;
    private string directory = "/LessonConfig/";
    private string jsonString;
    private JSONNode config;
    private float G;
    public void SetSceneToConfig()
    {
        DeleteAllGravityBody();
        config = GetJsonNode();

        // Create scene objects
        foreach (JSONNode node in config["bodies"])
        {
            CreateGravityBody(node);
        }
    }

    public void SetGravConst()
    {
        if (config == null)
        {
            config = GetJsonNode();
        }
        G = config["constants"]["G"].AsFloat;
        GravityBody.G_Runtime = G;
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

        GameObject gObj = GravityBody.CreateGravityBody(name, position, mass, 10, MaterialColours.Colours.BLUE);
    }

    public void ProcessJson(String json)
    {
        jsonString = json;
        config = JSON.Parse(jsonString);
        Debug.Log(jsonString);
    }

    private JSONNode GetJsonNode()
    {
        string projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
        string fullPath = projectPath + directory + fileName;
        if (File.Exists(fullPath))
        {
            return JSON.Parse(File.ReadAllText(fullPath));
        }
        else
        {
            Debug.Log(String.Format("Couldn't find filepath {0}", fullPath));
            return null;
        }
    }
}