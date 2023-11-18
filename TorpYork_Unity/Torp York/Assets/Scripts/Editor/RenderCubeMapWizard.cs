using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenderCubeMapWizard : ScriptableWizard
{
    public Camera camera;

    private void OnWizardUpdate()
    {
        helpString = "Assign the camera to render a Cubemap from";
        isValid = camera != null;
    }

    private void OnWizardCreate()
    {
        Cubemap cubemap = new Cubemap(512, TextureFormat.ARGB32, false);
        camera.RenderToCubemap(cubemap);
        AssetDatabase.CreateAsset(cubemap, $"Assets/Cubemaps/{camera.name}.cubemap");
    }

    [MenuItem("Toolbox/Render Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard<RenderCubeMapWizard>("Render Cubemap", "Render");
    }
}
