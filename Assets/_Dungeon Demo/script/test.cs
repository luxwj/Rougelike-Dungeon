using UnityEngine;
using System.Collections;
using System;

public class test : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Action<Transform,Vector3> _Instantiate=(t,v)=>{Instantiate(t,v,t.rotation);};
		//Tile[,] _map;
		Tile[,] _map=new Tile[4,4];
		_map [0, 1] = Tile.Wall_s;
		_map [0, 2] = Tile.Wall_s;
		_map [1, 1] = Tile.DirtFloor;
		_map [1, 2] = Tile.DirtFloor;
		_map [2, 1] = Tile.DirtFloor;
		_map [2, 2] = Tile.DirtFloor;
		_map [3, 1] = Tile.Corridor_ws;
		_map [3, 2] = Tile.Wall_w;
		_map [1, 3] = Tile.Corridor_ad;
		_map [2, 3] = Tile.Wall_d;
		_map [1, 0] = Tile.Corridor_ad;
		_map [2, 0] = Tile.Wall_a;
		//Instantiate (Resources.Load ("Floor_1_Prefab",typeof (Transform)) as Transform);
		creat_dungeon.Instance.creat (_map, _Instantiate);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
