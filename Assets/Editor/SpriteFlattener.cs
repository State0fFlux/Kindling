using UnityEditor;
using UnityEngine;
using System.IO;

public class SpriteFlattener : MonoBehaviour
{
    [MenuItem("Tools/Flatten To Sprite")]
    static void FlattenSelectedToSprite()
    {
        GameObject target = Selection.activeGameObject;
        if (!target)
        {
            Debug.LogError("No GameObject selected!");
            return;
        }

        Bounds bounds = GetBounds(target);
        float pixelsPerUnit = 100f; // adjust for your art scale
        int texWidth = Mathf.CeilToInt(bounds.size.x * pixelsPerUnit);
        int texHeight = Mathf.CeilToInt(bounds.size.y * pixelsPerUnit);

        RenderTexture rt = new RenderTexture(texWidth, texHeight, 24);
        Camera cam = new GameObject("TempCam").AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = bounds.extents.y;
        cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // transparent
        cam.targetTexture = rt;

        cam.cullingMask = LayerMask.GetMask("Default"); // adjust if needed

        cam.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        tex.Apply();

        RenderTexture.active = null;
        Object.DestroyImmediate(cam.gameObject);
        rt.Release();

        // Save to Assets folder
        string path = "Assets/Flattened_" + target.name + ".png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.Refresh();

        // Set texture import settings
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.alphaIsTransparency = true;
            importer.spritePixelsPerUnit = pixelsPerUnit;
            importer.SaveAndReimport();
        }

        Debug.Log("Flattened sprite saved to " + path);
    }

    static Bounds GetBounds(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(obj.transform.position, Vector3.one);
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);
        return bounds;
    }
}
