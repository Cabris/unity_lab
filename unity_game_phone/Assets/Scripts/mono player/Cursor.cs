using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
	[SerializeField]
	Camera uicamera;
	[SerializeField]
	Vector3 pos;
	//[SerializeField]
	//float g=.1f;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		Vector3 p= new Vector3(Screen.width/2,
		                       Screen.height/2, uicamera.nearClipPlane+.1f);
	 	pos=uicamera.ScreenToWorldPoint(p);
		transform.position=pos;
	}
}
