using UnityEngine;
using System.Collections;

public class creat_room{
	private static creat_room _instance;
	private creat_room(){
	}
	/// <summary>
	/// 单例
	/// </summary>
	/// <value>The instance.</value>
	public static creat_room Instance{
		get 
		{
			if (_instance == null) 
			{
				_instance = new creat_room ();
			}
			return _instance;
		}
	}
	private bool _first=true;//是否是第一次生成

}
