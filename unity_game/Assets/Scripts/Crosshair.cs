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
		for(int i=0;i<sprites.Count;i++){
			SpriteRenderer r=sprites[i];
			r.enabled=(i==index);
		}
	}
}
