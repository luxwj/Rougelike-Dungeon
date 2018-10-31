using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public Tile[,] mapTiles;

    [SerializeField]
    private int mapWidth = 100;
    [SerializeField]
    private int mapHeight = 100;
    [SerializeField]
    private int roomMaxWidth = 30;
    [SerializeField]
    private int roomMaxHeight = 30;
    [SerializeField]
    private int roomMinWidth = 5;
    [SerializeField]
    private int roomMinHeight = 5;
    [SerializeField]
    private int roomCounts;

    private void Start()
    {
        Builder mapBuilder = new Builder(floorPrefab, wallPrefab);
        mapTiles = new Tile[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; ++i){
            for (int j = 0; j < mapHeight; ++j){
                mapTiles[i, j] = new Tile(i, j, -1, false);
            }
        }

        mapBuilder.BuildMap(RandomPosRoomGen(roomCounts));

    }

    private int[,] RandomPosRoomGen(int roomNumber)
    {
        int[,] map = new int[100, 100];

        for (int i = 0; i < roomNumber; ++i)
        {
            bool canGenRoom = false;
            while (!canGenRoom)
            {
                int cRoomPosX = Random.Range(1, mapWidth - roomMaxWidth);
                int cRoomPosY = Random.Range(1, mapHeight - roomMaxHeight);
                int cRoomSizeX = Random.Range(roomMinWidth, roomMaxWidth);
                int cRoomSizeY = Random.Range(roomMinHeight, roomMaxHeight);

                for (int x = cRoomPosX; x < cRoomPosX + cRoomSizeX; ++x)
                {
                    for (int y = cRoomPosY; y < cRoomPosY + cRoomSizeY; ++y)
                    {
                        if (mapTiles[x, y].IsUsed())
                        {
                            canGenRoom = false;
                            break;
                        }
                        else  canGenRoom = true;
                    }
                    if (!canGenRoom)    break;
                }
                if (canGenRoom)
                {
                    for (int x = cRoomPosX; x < cRoomPosX + cRoomSizeX; ++x)
                    {
                        for (int y = cRoomPosY; y < cRoomPosY + cRoomSizeY; ++y)
                        {
                            if (x == cRoomPosX || x == cRoomPosX + cRoomSizeX - 1 || y == cRoomPosY || y == cRoomPosY + cRoomSizeY - 1)
                            {
                                mapTiles[x, y].SetWall();
                                map[x, y] = 1;
                            }
                            else
                            {
                                mapTiles[x, y].SetFloor();
                                map[x, y] = 0;
                            }
                        }
                    }
                }
            }
        }
        return map;
    }

    public class Tile {
        int posX;
        int posY;
        int isWall;     //1 = wall
        bool isUsed = false;

        public Tile (int x, int y, int isW, bool isU){
            posX = x;
            posY = y;
            isWall = isW;
            isUsed = isU;
        }

        public bool IsUsed (){
            return isUsed;
        }

        public void SetWall (){
            isWall = 1;
            isUsed = true;
        }

        public void SetFloor()
        {
            isWall = 0;
            isUsed = true;
        }
    }

    public class Builder {
        public GameObject floorPrefab;     //0 = floor
        public GameObject wallPrefab;      //1 = wall

        public Builder (GameObject fPrefab, GameObject wPrefab){
            floorPrefab = fPrefab;
            wallPrefab = wPrefab;
        }

        public void BuildFloor(int x, int y){
            Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.Euler(-90f, 0, 0));
        }

        public void BuildWall(int x, int y){
            Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
        }

        public void BuildMap(int[,] map){
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            for (int i = 0; i < width; ++i){
                for (int j = 0; j < height; ++j){
                    if (map[i, j] == 1){
                        BuildWall(i, j);
                    }else{
                        BuildFloor(i, j);
                    }
                }
            }
        }


    }
}
