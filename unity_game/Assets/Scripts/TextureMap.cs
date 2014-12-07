using UnityEngine;
using System.Collections;

public class TextureMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 scale=new Vector2(transform.lossyScale.x,transform.lossyScale.z);
		renderer.material.SetTextureScale("_MainTex",scale);
		renderer.material.SetTextureScale("_BumpMap",scale);
	}
}
