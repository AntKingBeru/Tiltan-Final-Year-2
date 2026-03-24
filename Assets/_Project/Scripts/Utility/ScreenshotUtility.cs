using UnityEngine;
using System.IO;

public static class ScreenshotUtility
{
    public static void Capture(Camera cam, int slot)
    {
        var rt = new RenderTexture(256, 256, 16);
        cam.targetTexture = rt;
        
        var texture = new Texture2D(256, 256, TextureFormat.RGB24, false);

        cam.Render();
        
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        texture.Apply();
        
        cam.targetTexture = null;
        RenderTexture.active = null;
        
        var bytes = texture.EncodeToPNG();

        var path = Path.Combine(Application.persistentDataPath, $"slot_{slot}.png");
        File.WriteAllBytes(path, bytes);
    }
}