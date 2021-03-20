using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{

    public UnityEngine.Camera camera;       

    private int resWidth;
    private int resHeight;
    string path;
    // Use this for initialization

    private static ScreenShot instance;
    public static ScreenShot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScreenShot>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (ScreenShot.Instance)
        {
            if (ScreenShot.Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);
        resWidth = 512;
        resHeight = 512;
    }


    void Start()
    {
        
        //Debug.Log(path);
    }

    [ContextMenu("Capture")]
    public Texture2D ClickScreenShot()
    {
        camera.gameObject.SetActive(true);
        //DirectoryInfo dir = new DirectoryInfo(path);
        //if (!dir.Exists)
        //{
        //    Directory.CreateDirectory(path);
        //}

        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);        
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        //byte[] bytes = screenShot.EncodeToPNG();
        //File.WriteAllBytes(name, bytes);
        camera.gameObject.SetActive(false);

        return screenShot;
    }
}