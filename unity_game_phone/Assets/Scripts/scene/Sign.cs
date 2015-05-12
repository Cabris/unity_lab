using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour {



	void Start () {
		//tt=GetComponentInChildren<TypogenicText>();
	}
	
	// Update is called once per frame
	void Update () {
		//tt.Text=text;
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Collider c=contact.otherCollider;
			string tag=c.gameObject.tag;
			//if(tag=="Player"){
				this.rigidbody.isKinematic=false;
				this.rigidbody.WakeUp();
			//}
		}
	}
}
