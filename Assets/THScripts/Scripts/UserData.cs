using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UserData : MonoBehaviour
{
    private static UserData instance;
    public static UserData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UserData>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Sprite mapSprite;
    public Sprite MapSprite
    {
        get { return mapSprite; }
    }

    [SerializeField]
    private User userData;
    public User GetUserData
    {
        get { return userData; }
    }

    private void Awake()
    {
        if(UserData.Instance)
        {
            if(UserData.Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        Load();
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            userData.Cost += 1000;
        }
    }

    public void SaveMapData()
    {
        Texture2D newScreenShot = ScreenShot.Instance.ClickScreenShot();
        MapData newData = new MapData(System.Convert.ToBase64String(newScreenShot.EncodeToPNG()));
        userData.mapDatas.Add(newData);
        newData.InitSprite();
        userData.OnMapDataChanged?.Invoke(userData.mapDatas);
    }

    public void SaveMapData(MapData data)
    {
        userData.mapDatas.Add(data);
        data.InitSprite();
        userData.OnMapDataChanged?.Invoke(userData.mapDatas);
    }

    public void Save()
    {
        BinarySerialize<User>(userData, "user1");
    }

    public void Load()
    {
        User user = BinaryDeserialize<User>("user1");

        if (user == null)
        {
            user = new User();

            Texture2D newScreenShot = ScreenShot.Instance.ClickScreenShot();
            user.mapDatas.Add(new MapData(System.Convert.ToBase64String(newScreenShot.EncodeToPNG())));
        }

        for(int i = 0; i < user.mapDatas.Count; i++)
        {
            user.mapDatas[i].InitSprite();
        }
        
        userData = user;
    }

    public static void BinarySerialize<T>(T t, string filename) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/" + filename, FileMode.Create);
        formatter.Serialize(stream, t);
        stream.Close();
    }

    public static T BinaryDeserialize<T>(string filename) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if(File.Exists(Application.dataPath + "/" + filename))
        {
            FileStream stream = new FileStream(Application.dataPath + "/" + filename, FileMode.Open);
            T t = (T)formatter.Deserialize(stream);
            stream.Close();
            return t;
        }
        

        return null;
    }

}

[System.Serializable]
public class User
{
    [SerializeField]
    private int cost;
    public int Cost
    {
        get { return cost; }
        set
        {
            cost = value;
        }
    }

    public User()
    {
        cost = 1000;
        mapDatas = new List<MapData>();
    }


    [SerializeField]
    public List<MapData> mapDatas;

    public delegate void MapDataChangeDelegate(List<MapData> datas);
    [System.NonSerialized]
    public MapDataChangeDelegate OnMapDataChanged;
}

[System.Serializable]
public class MapData
{
    [SerializeField]
    private string spriteStr;
    public string SpriteStr
    {
        get { return spriteStr; }
        set
        {
            spriteStr = value;
        }
    }

    [System.NonSerialized]
    public Sprite sprite;

    public void InitSprite()
    {
        Texture2D mapTexture;
        mapTexture = new Texture2D(1, 1);
        mapTexture.LoadImage(System.Convert.FromBase64String(spriteStr));
        mapTexture.Apply();
        this.sprite = Sprite.Create(mapTexture, new Rect(0, 0, mapTexture.width, mapTexture.height), Vector2.one * 0.5f);
    }

    public MapData(string spriteStr)
    {
        this.spriteStr = spriteStr;
    }
}