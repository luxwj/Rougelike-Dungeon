using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class creat_dungeon{
	private static creat_dungeon _instance;
	private float floor_length=1.518111f;
	private Transform floor,pillar,wall_ws,wall_ad;
	private creat_dungeon(){
		floor = Resources.Load ("Floor_1_Prefab",typeof (Transform)) as Transform;
		pillar = Resources.Load ("Pillar_1_Prefab",typeof (Transform))as Transform;
		wall_ws = Resources.Load ("Wall_w", typeof(Transform))as Transform;
		wall_ad = Resources.Load ("Wall_a", typeof(Transform))as Transform;
	}
	public static creat_dungeon Instance{
		get 
		{
			if (_instance == null) 
			{
				_instance = new creat_dungeon ();
			}
			return _instance;
		}
	}
	/// <summary>
	/// 根据二维数组实例化地牢
	/// </summary>
	/// <param name="t">T.</param>
	public void creat(Tile[,] map,Action<Transform,Vector3> Instantiate){
		for (int i = 0; i < map.GetLength (0); i++)
			for (int j = 0; j < map.GetLength (1); j++) {
				switch (map [i, j]) {
				case (Tile.DirtFloor):
					Instantiate (floor,new Vector3(i*floor_length,0,j*floor_length));
					break;
				case (Tile.Wall_s):
					Instantiate (wall_ws,new Vector3((i+0.5f)*floor_length,0,j*floor_length));
					Instantiate (pillar,new Vector3((i+0.5f)*floor_length,0,(j+0.5f)*floor_length));
					break;
				case (Tile.Wall_w):
					Instantiate (wall_ws,new Vector3((i-0.5f)*floor_length,0,j*floor_length));
					Instantiate (pillar,new Vector3((i-0.5f)*floor_length,0,(j-0.5f)*floor_length));
					break;
				case (Tile.Wall_a):
					Instantiate (wall_ad,new Vector3((i)*floor_length,0,(j+0.5f)*floor_length));
					Instantiate (pillar,new Vector3((i-0.5f)*floor_length,0,(j+0.5f)*floor_length));
					break;
				case (Tile.Wall_d):
					Instantiate (wall_ad,new Vector3((i)*floor_length,0,(j-0.5f)*floor_length));
					Instantiate (pillar,new Vector3((i+0.5f)*floor_length,0,(j-0.5f)*floor_length));
					break;
				case (Tile.Corridor_ad):
					Instantiate (floor,new Vector3(i*floor_length,0,j*floor_length));
					Instantiate (wall_ws,new Vector3((i+0.5f)*floor_length,0,j*floor_length));
					Instantiate (wall_ws,new Vector3((i-0.5f)*floor_length,0,j*floor_length));
					Instantiate (pillar,new Vector3((i-0.5f)*floor_length,0,(j+0.5f)*floor_length));
					Instantiate (pillar,new Vector3((i+0.5f)*floor_length,0,(j-0.5f)*floor_length));
					break;
				case (Tile.Corridor_ws):
					Instantiate (floor,new Vector3(i*floor_length,0,j*floor_length));
					Instantiate (wall_ad,new Vector3(i*floor_length,0,(j+0.5f)*floor_length));
					Instantiate (wall_ad,new Vector3(i*floor_length,0,(j-0.5f)*floor_length));
					Instantiate (pillar,new Vector3((i+0.5f)*floor_length,0,(j+0.5f)*floor_length));
					Instantiate (pillar,new Vector3((i-0.5f)*floor_length,0,(j-0.5f)*floor_length));
					break;
				default:
					break;
				}
			}
	}
}
