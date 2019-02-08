using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    //基本情報
    public Vector2Int Pos;
    public MapStatus Status;
}

public class RoomData : MapData
{
    //Roomの情報
    public int roomNum;
    public int Width;
    public int Height;
}

public class RoadData : MapData
{   
    //Raodの情報
    int StartRoomNum;
    int EndRoomNum;
    Vector2Int StartPos;
    Vector2Int EndPos;
}

public class GameManager : MonoBehaviour {

    public MapData[,] Map;
    public RoomData[,] roomData;
    public RoadData[,] roadData;

    public Vector2Int PlayerPos;
    public Vector2Int GorlPos;


    //始点から目的地までを探索し、ルートをvecter2Intのリストで返す
    //注意：リストの中身は終点から始点へのルートになっている。
    public List<Vector2Int> ASter(GameObject _from, Vector2Int _gorlPos)
    {
        ASterArg _aster = GetComponent<ASterArg>();
        Vector2Int FromPos = GetPosData(_from);

        return _aster.ASterkit(FromPos, _gorlPos);
    }

    //キャラクターが自分の座標を取得するのに使う
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
