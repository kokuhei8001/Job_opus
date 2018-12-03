using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour {

    [SerializeField] private GameObject WallObject;  //壁のオブジェクト
    [SerializeField] private GameObject GroundObject;//地面のオブジェクト
    [SerializeField] private GameObject Parent;      //大量のオブジェが生産されるので適当なオブジェの階層下で生成させる

    int MapWidth = 50;
    int MapHeight = 50;

    int[,] Map;

    //enumにする
    int wall = 9;
    int road = 0;

    int roomMinHeight = 5;
    int roomMaxHeight = 10;

    int roomMinWidth = 5;
    int roomMaxWidth = 10;

    int RoomCountMin = 10;
    int RoomCountMax = 15;

    //道の集合地点を増やしたいならこれを増やす
    int meetPointCount = 1;

    void Start()
    {
        ResetMapData();
        CreateSpaceData();
        CreateDangeon();
    }

    private void ResetMapData()
    {
        Map = new int[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                Map[i, j] = wall;
            }
        }
    }

    private void CreateSpaceData()
    {
        int roomCount = Random.Range(RoomCountMin, RoomCountMax);

        int[] meetPointsX = new int[meetPointCount];
        int[] meetPointsY = new int[meetPointCount];
        for (int i = 0; i < meetPointsX.Length; i++)
        {
            meetPointsX[i] = Random.Range(MapWidth / 4, MapWidth * 3 / 4);
            meetPointsY[i] = Random.Range(MapHeight / 4, MapHeight * 3 / 4);
            Map[meetPointsY[i], meetPointsX[i]] = road;
        }

        for (int i = 0; i < roomCount; i++)
        {
            int roomHeight = Random.Range(roomMinHeight, roomMaxHeight);
            int roomWidth = Random.Range(roomMinWidth, roomMaxWidth);
            int roomPointX = Random.Range(2, MapWidth - roomMaxWidth - 2);
            int roomPointY = Random.Range(2, MapWidth - roomMaxWidth - 2);

            int roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);
            int roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

            bool isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY);

            if (isRoad == false)
            {
                CreateRoadData(roadStartPointX, roadStartPointY, meetPointsX[Random.Range(0, 0)], meetPointsY[Random.Range(0, 0)]);
            }
        }


    }

    private bool CreateRoomData(int roomHeight, int roomWidth, int roomPointX, int roomPointY)
    {
        bool isRoad = false;
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                if (Map[roomPointY + i, roomPointX + j] == road)
                {
                    isRoad = true;
                }
                else
                {
                    Map[roomPointY + i, roomPointX + j] = road;
                }
            }
        }
        return isRoad;
    }

    private void CreateRoadData(int roadStartPointX, int roadStartPointY, int meetPointX, int meetPointY)
    {

        bool isRight;
        if (roadStartPointX > meetPointX)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        bool isUnder;
        if (roadStartPointY > meetPointY)
        {
            isUnder = false;
        }
        else
        {
            isUnder = true;
        }

        if (Random.Range(0, 2) == 0)
        {

            while (roadStartPointX != meetPointX)
            {

                Map[roadStartPointY, roadStartPointX] = road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }

            }

            while (roadStartPointY != meetPointY)
            {

                Map[roadStartPointY, roadStartPointX] = road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }

        }
        else
        {

            while (roadStartPointY != meetPointY)
            {

                Map[roadStartPointY, roadStartPointX] = road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }

            while (roadStartPointX != meetPointX)
            {

                Map[roadStartPointY, roadStartPointX] = road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }

            }

        }
    }

    private void CreateDangeon()
    {
        for (int i = 0; i < MapHeight; i++){
            for (int j = 0; j < MapWidth; j++){
                if (Parent != null)
                {
                    if (Map[i, j] == wall)
                    {
                        Instantiate(WallObject, new Vector3(j * 1.1f, 0, i * 1.1f), Quaternion.identity, Parent.transform);
                    }
                    Instantiate(GroundObject, new Vector3(j * 1.1f, -1, i * 1.1f), Quaternion.identity, Parent.transform);
                }
                else
                {
                    if (Map[i, j] == wall)
                    {
                        Instantiate(WallObject, new Vector3(j * 1.1f, 0, i * 1.1f), Quaternion.identity);
                    }
                    Instantiate(GroundObject, new Vector3(j * 1.1f, -1, i * 1.1f), Quaternion.identity);
                }
            }
        }
    }
}