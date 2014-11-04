using UnityEngine;
using System.Collections;

public class DamageObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			if(contact.otherCollider.gameObject.tag=="Player")
				contact.otherCollider.gameObject.GetComponent<PlayerBeheaver>().DoDamage(100);
		}

		
	}

}
