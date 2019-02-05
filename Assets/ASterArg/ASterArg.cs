using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ASterStatus
{
    Open,
    Close
}

[System.Serializable]
public class ASterData
{
    public Vector2Int Pos;
    public int Cost = 0;
    public int GessCost = 0;
    public int TotalCost = 0;
    public ASterStatus OpenData = ASterStatus.Open;
    public Vector2Int Parent;
    public bool Rout = false;
}

public class ASterArg : MonoBehaviour {

    //Mapのデータを持ってくる
    [SerializeField] MapCreate _mapCreate; //Mapの情報が格納されているscript
    private MapStatus[,] MapData;
    private ASterData[,] MapASterData;
    private List<ASterData> ASterOpenList; //Open状態になっている場所を格納する場所
    private List<Vector2Int> RoutData; //目的地までの道順
    private int ASterCount; //A*を走らせた回数

    private Vector2Int StartPos = new Vector2Int(0,0);
    private Vector2Int GorlPos = new Vector2Int(0, 0);

    //Asterループ管理
    private bool Surch;

    public List<Vector2Int> ASterkit(Vector2Int _StartPos,Vector2Int _EndPos)
    {
        //mapDataの初期化
        MapData = new MapStatus[_mapCreate.MapWidth, _mapCreate.MapHeight]; //Mapの情報を持ってくる
        MapASterData = new ASterData[_mapCreate.MapWidth, _mapCreate.MapHeight];
        for (int x = 0; x < _mapCreate.MapWidth; x++){
            for (int y = 0; y < _mapCreate.MapHeight; y++)
            {
                MapData[x,y] = _mapCreate.Map[x,y];

                MapASterData[x, y] = new ASterData();
                MapASterData[x, y].Pos = new Vector2Int(x, y);
            }
        }
        //その他データの初期化
        StartPos = _StartPos;
        GorlPos = _EndPos;

        ASterCount = 0;
        ASterOpenList = new List<ASterData>();
        RoutData = new List<Vector2Int>();
        Surch = true;
        

        //Aster
        ASter(StartPos,GorlPos);
        while (Surch)
        {
            ASter(ASterOpenList[0].Pos, GorlPos);
        }
        return RoutData;//逆から入れる？
    }

    private void ASter(Vector2Int Pos,Vector2Int Gorl)
    {
        //自分の場所を探索済みにする
        MapASterData[Pos.x, Pos.y].OpenData = ASterStatus.Close;
        if (ASterOpenList.Count != 0) ASterOpenList.RemoveAt(0);

        //探索する向き
        Vector2Int direction = new Vector2Int(0, 0);

        for (int i = 0; i < 4; i++)
        {
            if (i == 0) direction = new Vector2Int(1, 0);  //右
            if (i == 1) direction = new Vector2Int(-1, 0);//左
            if (i == 2) direction = new Vector2Int(0, 1);  //上
            if (i == 3) direction = new Vector2Int(0, -1);//下

            int x = Pos.x + direction.x;
            int y = Pos.y + direction.y;

            if (x < 0 || y < 0)
            {
                continue;
            }
            else if (x >= _mapCreate.MapWidth || y >= _mapCreate.MapHeight)
            {
                continue;
            }
            
            //目的地に到達したら
            if (new Vector2Int(x, y) == Gorl)
            {
                //目的地の親を現在地にする
                MapASterData[x, y].Parent = Pos;

                //親をたどり開始位置まで戻る(Listに入れる)
                Vector2Int tempPos = new Vector2Int(x, y);
                while (true)
                {
                    RoutData.Add(tempPos);
                    MapASterData[tempPos.x, tempPos.y].Rout = true;

                    if (MapASterData[tempPos.x, tempPos.y].Parent == StartPos)
                    {
                        break;
                    }
                    else
                    {
                        tempPos = MapASterData[tempPos.x, tempPos.y].Parent;
                    }
                }
                Debug.Log("目的地を見つけました");
                Surch = false; //A*を中止させる
                break;//このfor文抜ける
            }

            if (MapData[x, y] != MapStatus.Wall) //壁以外だったら
            {
                if (MapASterData[x, y].OpenData == ASterStatus.Open)
                {
                    //目的地の親を現在地にする
                    MapASterData[x, y].Parent = Pos;

                    //GessCostを取得する
                    MapASterData[x, y].Cost = MapASterData[Pos.x,Pos.y].Cost + 1;
                    MapASterData[x, y].GessCost = CalculationCost(new Vector2Int(x, y),Gorl);
                    MapASterData[x, y].TotalCost = MapASterData[x, y].Cost + MapASterData[x, y].GessCost;

                    //これをすることで大きく速度が変わる（要検証）
                    //MapASterData[x, y].OpenData = ASterStatus.Close;

                    ASterOpenList.Add(MapASterData[x, y]);
                }
            }
        }//for(i < 4)

        if (ASterOpenList.Count == 0)
        {
            Debug.Log("経路探索が失敗しました");
            Surch = false;
        }

        ListSort();
        ASterCount++;
        
    }//ASterArg
    

    //目的地までの推定コスト
    private int CalculationCost(Vector2Int Pos, Vector2Int End)
    {
        Vector2Int answer = new Vector2Int(0, 0);

        answer.x = Pos.x - End.x;
        if (answer.x < 0)
        {
            answer.x *= -1;
        }
        answer.y = Pos.y - End.y;
        if (answer.y < 0)
        {
            answer.y *= -1;
        }

        return answer.x + answer.y;
    }

    //Listの中身をソートする
    private void ListSort()
    {
        ASterOpenList.Sort(
            (a, b) => a.TotalCost - b.TotalCost
            );
    }
}
