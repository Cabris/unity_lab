using UnityEngine;
using System.Collections;

public class buster : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	public float scrollSpeed = 0.5F;
	// Update is called once per frame
	void Update () {
		float offset = Time.time * scrollSpeed;
		renderer.material.mainTextureOffset = new Vector2(0, -offset);
	}
}
