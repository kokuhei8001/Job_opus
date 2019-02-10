using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public int SaveDataVersion = 0;
    public Vector2Int ScoreTime = new Vector2Int(0,0);
}

[SerializeField]
public class JsonPacker : MonoBehaviour
{
    SaveData _data;

    static string FilePath;

    private void Awake()
    {
        FilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
    }

    private void Start()
    {
        _data = LoadFromJson();

        //もしデータがあるなら
        if (_data != null)
        {
        }
    }

    private void Update()
    {
        //とりあえずSaveDataに何もなかったら何もやらない.
        if (_data == null) return;                  
    }

    //アプリケーションが終了時（タスクキル）に呼び出される
    private void OnApplicationQuit()
    {
        //SaveToJson(SaveData.FilePath, _data); //このプログラムが走っているとデバッグ作業だと逆にデータが初期化される事があるので書き方に注意
    }

    //セーブ（ファイルを書き込む）
    public static void SaveToJson(SaveData data)
    {
        File.WriteAllText(FilePath, JsonUtility.ToJson(data));
    }

    //ロード（ファイルを読み込む）
    public static SaveData LoadFromJson()
    {
        if (!File.Exists(FilePath))//ファイルがない場合
        {
            SaveData NewData = new SaveData();

            return NewData;
        }

        string sd = File.ReadAllText(FilePath); //filePathだとnullになる。Application.persistentDataPathだとアクセスが許可されていないになる
        
        return JsonUtility.FromJson<SaveData>(sd);
        //return JsonUtility.FromJson(sd, typeof(SaveData)) as SaveData;
    }
}