using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Motion
{
    Idle = 0,
    Walk = 1,
    Run = 2,
    WalkLeft = 11,
    WalkRight = 12,
    RunLeft = 21,
    RunRight = 22,

    SpotLeft = -1,
    SpotRigh = -2,

    Back = -9
}

public class MapData
{
    //基本情報
    public Vector2Int Pos;
    public MapStatus Status;
}

public class Room : MapData
{
    //Roomの情報
    public int Num;
    public Vector2Int Size; // Vector2Int( width , height );
}

public class GameManager : MonoBehaviour {

    public MapData[,] Map;
    public Room[] Room;

    public Vector2Int PlayerPos;
    public Vector2Int GorlPos;

    //TimeScore
    public Vector2Int Score;
    private float _second = 0;
    
    private void Update()
    {
        //プレイ中の時間（スコア）
        _second += Time.deltaTime;
        if (_second > 60)
        {
            Score.x++;
            _second = 0;
        }

        Score = new Vector2Int(Score.x, (int)_second);
    }

    //始点から目的地までを探索し、ルートをvecter2Intのリストで返す
    //注意：リストの中身は終点から始点へのルートになっている。
    public List<Vector2Int> ASter(GameObject _from, Vector2Int _gorlPos)
    {
        ASterArg _aster = GetComponent<ASterArg>();
        Vector2Int FromPos = GetPosData(_from);

        return _aster.ASterkit(FromPos, _gorlPos);
    }

    //キャラクターがRayを足元に飛ばして自分の座標を取得するのに使う
    public Vector2Int GetPosData(GameObject myself)
    {
        Vector3 under = new Vector3(0, -90, 0);
        Vector3 myposition = new Vector3(myself.transform.position.x, myself.transform.position.y + 0.2f, myself.transform.position.z);

        Ray ray = new Ray(myposition, under);
        RaycastHit hit;
        float distance = 2.0f;
        //Debug
        Debug.DrawRay(myposition, under * distance, Color.blue);

        if (Physics.Raycast(ray, out hit, distance))
        {
            GroundData answer = hit.collider.GetComponent<GroundData>();
            if (answer != null)
            {
                return new Vector2Int(answer.PosX, answer.PosY);
            }
        }
        return new Vector2Int();
    }

}
