using UnityEngine;
using System.Collections;

public class doorcontroll : MonoBehaviour {
	Transform player,m_transform;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		m_transform=this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		float d = (m_transform.position - player.position).magnitude;
		if ((Input.GetKey (KeyCode.Space)) && (d < 1f))
			opendoor ();
	}
	void opendoor(){
		m_transform.GetComponent<Animation> ().Play();
	}

}
