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


    public void onClickSnapShot()

    {
        DoSnapshot(TargetSnap, canvas, camShot);
        StartCoroutine(WaitReturnImg());
    }
    IEnumerator WaitReturnImg()
    {
        yield return new WaitForSeconds(2);
        var tex = LoadPNG(Application.dataPath + "/Prefabs/snapshots/.png");
        Debug.Log(Application.dataPath + "/Prefabs/snapshots/.png");
        Debug.Log(tex);
        var sprite = CreateSprite(tex);
        ChangeImg(sprite);
    }

    private static void DoSnapshot(GameObject go, Canvas canvas, Camera cam)
    {
        var ins = GameObject.Instantiate(go, canvas.transform, false);

        ins.SetActive(true);

        string fileName = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(go)) + ".png";
        string astPath = "Assets/Prefabs/snapshots/" + fileName;
        fileName = Application.dataPath + "/Prefabs/snapshots/" + fileName;
        FileInfo info = new FileInfo(fileName);
        if (info.Exists)
            File.Delete(fileName);
        else if (!info.Directory.Exists)
            info.Directory.Create();

        var renderTarget = RenderTexture.GetTemporary(1080, 1920);
        cam.aspect = 1 / (1920.0f / 1080f);
        cam.orthographic = true;
        cam.targetTexture = renderTarget;
        cam.Render();

        RenderTexture.active = renderTarget;
        Texture2D tex = new Texture2D(renderTarget.width, renderTarget.height);
        tex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);

        File.WriteAllBytes(fileName, tex.EncodeToPNG());

        cam.targetTexture = null;
        Object.DestroyImmediate(ins);
        cam.gameObject.SetActive(false);
        var sprite = CreateSprite(tex);

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
