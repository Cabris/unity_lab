using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
	[SerializeField]
	Camera uicamera;
	[SerializeField]
	Vector3 pos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 pos=uicamera.ScreenToWorldPoint(Input.mousePosition);
		transform.position=pos;
	}
}
