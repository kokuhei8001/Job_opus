using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public int SaveDataVersion = 0;
    public List<Vector2Int> TimeRanking = new List<Vector2Int>();
}

[SerializeField]
public class JsonPacker : MonoBehaviour
{
    public SaveData _data;

    static string FilePath;

    private void Awake()
    {
        FilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        _data = LoadFromJson();
    }

    //セーブ（ファイルを書き込む）
    public void SaveToJson(SaveData data , Vector2Int Score)
    {
        data.TimeRanking.Add(Score);

        for (int i = 0;i < data.TimeRanking.Count; i++)
        {
            if (data.TimeRanking[i].x < data.TimeRanking[i + 1].x)
            {
                Vector2Int temp = data.TimeRanking[i];
                data.TimeRanking[i] = data.TimeRanking[i + 1];
                data.TimeRanking[i + 1] = temp;
            }
            else
            { break; }
        }
        for (int i = 0; i < data.TimeRanking.Count; i++)
        {
            if (data.TimeRanking[i].y < data.TimeRanking[i + 1].y)
            {
                Vector2Int temp = data.TimeRanking[i];
                data.TimeRanking[i] = data.TimeRanking[i + 1];
                data.TimeRanking[i + 1] = temp;
            }
            else
            { break; }
        }
        //for (int i = data.TimeRanking.Count - 1; i < -1; i--)
        //{
        //    if (data.TimeRanking[i].x > data.TimeRanking[i - 1].x)
        //    {
        //        Vector2Int temp = data.TimeRanking[i];
        //        data.TimeRanking[i] = data.TimeRanking[i - 1];
        //        data.TimeRanking[i - 1] = temp;
        //    }
        //    else
        //    { break; }
        //}
        //for (int i = data.TimeRanking.Count - 1; i < -1; i--)
        //{
        //    if (data.TimeRanking[i].y > data.TimeRanking[i - 1].y)
        //    {
        //        Vector2Int temp = data.TimeRanking[i];
        //        data.TimeRanking[i] = data.TimeRanking[i - 1];
        //        data.TimeRanking[i - 1] = temp;
        //    }
        //    else
        //    { break; }
        //}

        if (data.TimeRanking.Count == 11)
        { data.TimeRanking.RemoveAt(10); }
        
        data.SaveDataVersion++;
        File.WriteAllText(FilePath, JsonUtility.ToJson(data));
    }

    //ロード（ファイルを読み込む）
    public SaveData LoadFromJson()
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