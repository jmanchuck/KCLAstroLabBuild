// Created by jmanchuck October 2020

using System;
using UnityEngine;
public class MaterialColours : MonoBehaviour
{

    public enum Colours
    {
        BLUE,
        GREEN,
        ORANGE,
        RED
    }
    public static Material GetMaterial(Colours colour)
    {
        Material mat;
        String matPath = "";
        switch (colour)
        {
            case Colours.BLUE:
                matPath = "GlowMaterials/glowBlue";
                break;

            case Colours.GREEN:
                matPath = "GlowMaterials/glowGreen";
                break;

            case Colours.ORANGE:
                matPath = "GlowMaterials/glowOrange";
                break;

            case Colours.RED:
                matPath = "GlowMaterials/glowRed";
                break;

        }

        mat = Resources.Load(matPath, typeof(Material)) as Material;

        return mat;
    }
}


