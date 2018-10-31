using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class t : MonoBehaviour {
	public InputField inputfield;
	public Text text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (text.text);
	}
}
