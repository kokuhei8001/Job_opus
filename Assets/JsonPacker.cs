using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{

    //セーブに必要な変数またはクラスを用意する
    public int Id = 0;
    public Vector2 pos = new Vector2(0, 0);

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
        //Debug.Log(SaveData.FilePath);
        _data = LoadFromJson();

        if (_data != null)
        {
            Debug.Log(_data.Id);
            Debug.Log("x:" + _data.pos.x + "y:" + _data.pos.y);
        }
    }

    private void Update()
    {
        if (_data == null) return;                  //とりあえずSaveDataに何もなかったら何もやらない.

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
            Debug.Log("FileEmpty!");

            SaveData test = new SaveData();

            return test;
        }

        string sd = File.ReadAllText(FilePath); //filePathだとnullになる。Application.persistentDataPathだとアクセスが許可されていないになる


        return JsonUtility.FromJson<SaveData>(sd);
        //return JsonUtility.FromJson(sd, typeof(SaveData)) as SaveData;
    }
}