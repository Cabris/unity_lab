using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {
	[SerializeField]
	Light light;
	Transform player;
	float intensity;
	
	// Use this for initialization
	void Start () {
		//light.enabled=false;
		player = GameObject.Find ("SceneLogic/local_player_standard").transform;
		intensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		float d = Vector3.Distance (player.position, transform.position);
		if (d < 15)
			light.intensity = intensity;
		else if (d >= 15 && d <= 20)//15-20
			light.intensity = (1f-((d - 15f)/5f)) * intensity;
		else
			light.intensity = 0;
	}
	
	//	void OnCollisionEnter(Collision collision) {
	//		if(collision.collider.gameObject.tag=="Player")
	//			light.enabled=true;
	//	}
	//	
	//	void OnCollisionExit(Collision collision) {
	//		if(collision.collider.gameObject.tag=="Player")
	//			light.enabled=false;
	//	}
}
