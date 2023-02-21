using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CamScreenShot : MonoBehaviour
{
    public Camera camShot;
    public Canvas canvas;
    public Image image;
    public GameObject TargetSnap;

    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
    public void onClickSnapShot()

    {
        DoSnapshot(TargetSnap, canvas, camShot);
        StartCoroutine(WaitReturnImg());
    }
    IEnumerator WaitReturnImg()
    {
        yield return new WaitForSeconds(2);
        var tex = LoadPNG(Application.dataPath + "/Prefabs/snapshots/.png");
        // Debug.Log("Application.dataPath: " + Application.dataPath + "/Prefabs/snapshots/.png");
        // Debug.Log("tex:" + tex);
        // Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
        var sprite = CreateSprite(tex);
        ChangeImg(sprite);
    }

    private void DoSnapshot(GameObject go, Canvas canvas, Camera cam)
    {
        // var ins = GameObject.Instantiate(go, canvas.transform, false);
        // ins.SetActive(true);
        // string fileName = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(go)) + ".png";
        // string astPath = "Assets/Prefabs/snapshots/" + fileName;
        // fileName = Application.dataPath + "/Prefabs/snapshots/" + fileName;
        // FileInfo info = new FileInfo(fileName);
        // if (info.Exists)
        //     File.Delete(fileName);
        // else if (!info.Directory.Exists)
        //     info.Directory.Create();

        // var renderTarget = RenderTexture.GetTemporary(1080, 1920);
        // cam.aspect = 1 / (1920.0f / 1080f);
        // cam.orthographic = true;
        // cam.targetTexture = renderTarget;
        // cam.Render();

        // RenderTexture.active = renderTarget;
        // Texture2D tex = new Texture2D(renderTarget.width, renderTarget.height);
        // tex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);

        // File.WriteAllBytes(fileName, tex.EncodeToPNG());

        // cam.targetTexture = null;
        // Object.DestroyImmediate(ins);
        // // cam.gameObject.SetActive(false);
        // var sprite = CreateSprite(tex);
        string fileName = Application.persistentDataPath + "testTexture.jpg";
        if (File.Exists(fileName))
        {
            Debug.Log("file exist");
            Debug.Log(fileName);

        }
        else
        {
            var texture = new Texture2D(512, 512);
            texture = (Texture2D)image.mainTexture;
            Texture2D decopmpresseTex = DeCompress(texture);
            byte[] bytes = decopmpresseTex.EncodeToJPG();
            File.WriteAllBytes(Application.persistentDataPath + "testTexture.jpg", bytes);
        }
    }
    public static class ExtensionMethod
    {

    }

    private void ChangeImg(Sprite targetImg)
    {
        image.sprite = targetImg;
        image.SetNativeSize();
    }
    private static Sprite CreateSprite(Texture2D tex)
    {
        var sprite = Sprite.Create(tex, new Rect(0, 0, 1080, 1920f), Vector2.up, 100.0f);
        return sprite;
    }
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1080, 1920);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
