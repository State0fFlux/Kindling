using UnityEditor;
using UnityEngine;
using System.IO;

public class WorldSpaceImageFlattener
{
    [MenuItem("Tools/Flatten Visible Images From Main Camera")]
    static void FlattenFromMainCamera()
    {
        Camera cam = Camera.main;
        if (!cam)
        {
            Debug.LogError("No camera tagged as 'MainCamera' found!");
            return;
        }

        int texWidth = Mathf.CeilToInt(cam.pixelWidth);
        int texHeight = Mathf.CeilToInt(cam.pixelHeight);
        RenderTexture rt = new RenderTexture(texWidth, texHeight, 24);
        cam.targetTexture = rt;

        // Make sure only Default layer is rendered
        int originalMask = cam.cullingMask;

        cam.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();

        cam.cullingMask = originalMask; // restore

        string path = "Assets/Flattened_MainCamera.png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.Refresh();

        // Import as sprite
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.alphaIsTransparency = true;
            importer.spritePixelsPerUnit = 100f;
            importer.SaveAndReimport();
        }

        Debug.Log("Snapshot flattened and saved to " + path);
    }
}
