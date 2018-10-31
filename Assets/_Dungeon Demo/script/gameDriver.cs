using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class gameDriver : MonoBehaviour {
	private Transform dungeon;
	public int random_seed=1;
	public int room_max_length=5,room_max_width=5,room_min_length=3,room_min_width=3,map_max_length=100,map_max_width=100,room_num=5,min_corridor_len=1,max_corridor_len=3,step=10000;//房间的最大最小宽度，地图的最大长宽，房间的个数
	private input ii;
	// Use this for initialization
	void Start () {
		dungeon = GameObject.Find ("dungeon").transform;//在地图上事先创建了一个放实体的空物体
		ii=this.transform.GetComponent<input>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if (GUI.Button (new Rect (Screen.width - 160, 10, 150, 50), "生成地牢")) {
			Action<Transform,Vector3> _Instantiate=(t,v)=>{object g=Instantiate(t,v,t.rotation);((Transform)g).SetParent(dungeon);};
			//UnityEngine.Random.InitState (random_seed);//初始化随机种子
			map.Instance.Init (room_max_length, room_max_width, room_min_length, room_min_width, map_max_length, map_max_width, room_num,min_corridor_len,max_corridor_len);//初始化参数
			map.Instance.make_dungeon(step);//生成地牢
			creat_dungeon.Instance.creat (map.Instance.getmap (), _Instantiate);//实体化地牢
		}
		if (GUI.Button (new Rect (Screen.width - 160, 70, 150, 50), "种子重置")) {
			random_seed =int.Parse(ii.random_seed.ToString());
			UnityEngine.Random.InitState (random_seed);//初始化随机种子
		}
		if (GUI.Button (new Rect (Screen.width - 160, 130, 150, 50), "地牢消除")) {
			foreach (Transform t in dungeon) {
				Destroy (t.gameObject);
			}
		}
	}
}
