using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Crosshair : MonoBehaviour {

	[SerializeField]
	List< SpriteRenderer> sprites;
	public int index;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

//		Camera uicamera=Camera.main;
//		Vector3 p= new Vector3(Screen.width/2,
//		                       Screen.height/2, uicamera.nearClipPlane+.1f);
//		Vector3 p2 = transform.position;
//		p2.z=uicamera.ScreenToWorldPoint(p).z;
//		transform.position = uicamera.ScreenToWorldPoint(p);


		for(int i=0;i<sprites.Count;i++){
			SpriteRenderer r=sprites[i];
			r.enabled=(i==index);
		}
	}
}
