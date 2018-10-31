using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorS : MonoBehaviour {

    /// <summary>
    /// The position (0, 0) is at the bottom left
    /// </summary>

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    private Room[,] mapRooms;
    private TileTypes[,] mapTiles;
    private MapBuilder mapBuilder;

    private enum RoomTypes{
        upstair, downstair, normal, statue, shop, treasure, none
    }
    /*
     * statues only appear at the corner, twice per map
     * shop only appears once a map
     * treasure only appears once a map and outside the main path, not at the corner
     */

    private enum Directions{
        left, right, up, down, none
    }

    private enum TileTypes{
        wall, floor, none
    }

    private int mapWidth;
    private int mapHeight;
    [SerializeField]
    private int roomWidth;
    [SerializeField]
    private int roomHeight;
    [SerializeField]
    private int roomWidthCount;
    [SerializeField]
    private int roomHeightCount;
    public int RoomWidth{
        get{
            return roomWidth;
        }
        set{
            roomWidth = (value != 0) ? value : 10;
        }
    }
    public int RoomHeight{
        get{
            return roomHeight;
        }
        set{
            roomHeight = (value != 0) ? value : 10;
        }
    }
    public int RoomWidthCount{
        get{
            return roomWidthCount;
        }
        set{
            roomWidthCount = (value != 0) ? value : 8;
        }
    }
    public int RoomHeightCount{
        get{
            return roomHeightCount;
        }
        set{
            roomHeightCount = (value != 0) ? value : 8;
        }
    }

    public List<int[]> normalRooms = new List<int[]>();

    private void Start(){
        mapWidth = roomWidth * roomWidthCount + 2;
        mapHeight = roomHeight * roomHeightCount + 2;

        mapBuilder = new MapBuilder(floorPrefab, wallPrefab);
        mapTiles = new TileTypes[mapWidth, mapHeight];
        mapRooms = new Room[RoomWidthCount, RoomHeightCount];

        for (int i = 0; i < roomWidthCount; ++i) {
            for (int j = 0; j < roomHeightCount; ++j) {
                mapRooms[i, j] = new Room();
            }
        }

        MapInit();
        GenerateRooms();
        mapBuilder.BuildMap(mapTiles);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            MapInit();
            GenerateRooms();
            mapBuilder.BuildMap(mapTiles);
        }
    }

    private void MapInit(){
        GameObject[] origMapTiles = GameObject.FindGameObjectsWithTag("MapTiles");
        foreach (GameObject origMapTile in origMapTiles){
            Destroy(origMapTile);
        }
        for (int i = 0; i < mapWidth; ++i) {
            for (int j = 0; j < mapHeight; ++j) {
                if (i == 0 || i == mapWidth - 1 || j == 0 || j == mapHeight - 1) {       //Edges
                    mapTiles[i, j] = TileTypes.wall;
                } else {
                    mapTiles[i, j] = TileTypes.none;
                }
            }
        }
        for (int i = 0; i < roomWidthCount; ++i) {
            for (int j = 0; j < roomHeightCount; ++j) {
                mapRooms[i, j].roomType = RoomTypes.none;
                mapRooms[i, j].SetFromDir(Directions.none);
            }
        }
    }

    private void GenerateRooms(){
        int x = GenerateUpstairRoom();
        int y = roomHeightCount - 1;
        while (y >= 0){
            int dir = Random.Range(0, 5);
            /*
             * 0 | 1 left
             * 2 | 3 right
             * 4     down
            */

            if ( (dir == 0 || dir == 1) && x > 0){                                    //left
                if (mapRooms[x - 1, y].roomType == RoomTypes.none){
                    mapRooms[x, y].SetToDir(Directions.left);
                    --x;
                    GenerateNormalRoom(x, y, Directions.right);
                }else
                    --x;
            } else if ( (dir == 2 || dir == 3) && x < roomWidthCount - 1){             //right
                if (mapRooms[x + 1, y].roomType == RoomTypes.none){
                    mapRooms[x, y].SetToDir(Directions.right);
                    ++x;
                    GenerateNormalRoom(x, y, Directions.left);
                }else
                    ++x;
            } else {                                                                    //down
                if (y == 0) {
                    mapRooms[x, y].SetToDir(Directions.none);
                    mapRooms[x, y].ChangeToDownstair();
                    FixWall(x);
                    --y;
                    normalRooms.RemoveAt(normalRooms.Count - 1);
                } else {
                    mapRooms[x, y].SetToDir(Directions.down);
                    --y;
                    GenerateNormalRoom(x, y, Directions.up);
                }
            }
        }
    }

    private void FixWall (int x){
        int offsetX = x * roomWidth + 1;
        for (int i = 0; i < roomWidth; ++i) {
            mapTiles[offsetX + i, 1] = TileTypes.wall;
        }
    }

    private int GenerateUpstairRoom() {
        int x = Random.Range(0, roomWidthCount);
        int y = roomHeightCount - 1;
        mapRooms[x, y].roomType = RoomTypes.upstair;
        int offsetX = x * roomWidth + 1, offsetY = y * roomHeight + 1;
        for (int i = 0; i < roomWidth; ++i){
            for (int j = 0; j < roomHeight; ++j) {
                if (i == 0 || i == roomWidth - 1 || j == 0 || j == roomHeight - 1){
                    mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                } else {
                    mapTiles[offsetX + i, offsetY + j] = TileTypes.floor;
                }
            }
        }
        return x;
    }

    private void GenerateNormalRoom(int x, int y, Directions fromDir) {
        mapRooms[x, y].roomType = RoomTypes.normal;
        mapRooms[x, y].SetFromDir(fromDir);
        int[] roomPos = { x, y };
        normalRooms.Add(roomPos);
        int offsetX = x * roomWidth + 1, offsetY = y * roomHeight + 1;
        if (fromDir == Directions.up){
            int doorPos = Random.Range(3, roomWidth - 3);       //door is 3 tiles wide
            for (int i = 0; i < roomWidth; ++i){
                for (int j = 0; j < roomHeight; ++j){
                    if (i == 0 || i == roomWidth - 1 || j == 0){          //check edge
                        mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                    } else if (j == roomHeight - 1){  //check edge
                        if (i < doorPos - 1 || i > doorPos + 1)
                            mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                        else {
                            mapTiles[offsetX + i, offsetY + j] = TileTypes.floor;
                            mapTiles[offsetX + i, offsetY + j + 1] = TileTypes.floor;
                        }  
                    } else {
                        mapTiles[offsetX + i, offsetY + j] = TileTypes.floor;
                    }
                }
            }
        }else{
            int doorPos = Random.Range(3, roomHeight - 3);       //door is 3 tiles wide
            for (int i = 0; i < roomWidth; ++i) {
                for (int j = 0; j < roomHeight; ++j) {
                    if (j == 0 || j == roomHeight - 1) {          //check edge
                        mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                    } else if ( (i == 0 && fromDir == Directions.left) || (i == roomWidth - 1 && fromDir == Directions.right) ) {    //check edge
                        if (j < doorPos - 1 || j > doorPos + 1)
                            mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                        else {
                            mapTiles[offsetX + i, offsetY + j] = TileTypes.floor;
                            if (fromDir == Directions.left)
                                mapTiles[offsetX + i - 1, offsetY + j] = TileTypes.floor;
                            else
                                mapTiles[offsetX + i + 1, offsetY + j] = TileTypes.floor;
                        }
                    } else if (i == 0 || i == roomWidth - 1){
                        mapTiles[offsetX + i, offsetY + j] = TileTypes.wall;
                    } else {
                        mapTiles[offsetX + i, offsetY + j] = TileTypes.floor;
                    }
                }
            }
        }
    }

    class Room {
        public RoomTypes roomType;
        Directions fromDir;
        List<Directions> toDir = new List<Directions>();

        public void SetFromDir (Directions m_fromDir){
            fromDir = m_fromDir;
        }
        public void SetToDir(Directions m_toDir){
            toDir.Add(m_toDir);
        }
        public void ChangeToDownstair(){
            roomType = RoomTypes.downstair;
        }
    }

    class MapBuilder
    {
        public GameObject floorPrefab;     //0 = floor
        public GameObject wallPrefab;      //1 = wall

        public MapBuilder(GameObject fPrefab, GameObject wPrefab)
        {
            floorPrefab = fPrefab;
            wallPrefab = wPrefab;
        }

        public void BuildFloor(int x, int y)
        {
            Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.Euler(-90f, 0, 0));
        }

        public void BuildWall(int x, int y)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
        }

        public void BuildMap(TileTypes[,] mapTiles)
        {
            int width = mapTiles.GetLength(0);
            int height = mapTiles.GetLength(1);
            for (int i = 0; i < width; ++i){
                for (int j = 0; j < height; ++j){
                    switch (mapTiles[i, j]){
                        case TileTypes.wall: {
                            BuildWall(i, j);
                            break;
                        }
                        case TileTypes.floor: {
                            BuildFloor(i, j);
                            break;
                        }
                        case TileTypes.none: {
                            BuildWall(i, j);
                            break;
                        }
                    }
                }
            }
        }


    }
}
