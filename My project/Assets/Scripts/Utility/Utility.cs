using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour 
{
    public static void ChangeColorMateMesh(Color color, Renderer renderer)
    {
        Material newMaterial = new(renderer.sharedMaterial)
        {
            color = color
        };

        renderer.material = newMaterial;
    }
}
