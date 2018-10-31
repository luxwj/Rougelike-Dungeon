using UnityEngine;
using System.Collections;


//控制人物的移动
public class move_controll : MonoBehaviour {
	Transform m_transform,m_camera;
	CharacterController controller;
	public float MoveSpeed = 2.0f;//移动的速度
	public static bool g=true;//是否应用重力
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		m_camera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		controller=GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey (KeyCode.W)) || (Input.GetKey (KeyCode.S)) || (Input.GetKey (KeyCode.A)) || (Input.GetKey (KeyCode.D))) {
			//transform.GetComponent<Animator>().SetFloat("speed", 8f);
			if (Input.GetKey (KeyCode.W)) {
				//根据主相机的朝向决定人物的移动方向，下同
				controller.transform.eulerAngles = new Vector3 (0, m_camera.transform.eulerAngles.y, 0);
			}

			if (Input.GetKey (KeyCode.S)) {
				controller.transform.eulerAngles = new Vector3 (0, m_camera.transform.eulerAngles.y+180f, 0);
			}

			if (Input.GetKey (KeyCode.A)) {
				controller.transform.eulerAngles = new Vector3 (0, m_camera.transform.eulerAngles.y+270f, 0);
			}

			if (Input.GetKey (KeyCode.D)) {
				controller.transform.eulerAngles = new Vector3 (0, m_camera.transform.eulerAngles.y+90f, 0);
			}

			controller.Move(m_transform.forward * Time.deltaTime * MoveSpeed);
		}
		else
			//静止状态
			//transform.GetComponent<Animator>().SetFloat("speed", 0f);
		if (Input.GetKey (KeyCode.Q)) {
			transform.Translate (Vector3.up * Time.deltaTime * MoveSpeed);
		}
		//如果场景加载完成就应用重力
		if ((!controller.isGrounded)&&(g==true)) {
			//模拟重力，每秒下降10米
			controller.Move(new Vector3(0,-10f*Time.deltaTime,0));
		}
	}
}
