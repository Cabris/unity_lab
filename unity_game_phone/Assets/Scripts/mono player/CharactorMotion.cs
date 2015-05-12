using UnityEngine;
using System.Collections;

public class CharactorMotion : MonoBehaviour {

	[SerializeField]
	Transform target;
	[SerializeField]
	Transform cameraAvator;
	[SerializeField]
	float speed=2.5f;
	void Start () {
		target.parent=transform.parent;
		PlayerBeheaver pb=GetComponent<PlayerBeheaver>();
		if(pb.controlType==PlayerBeheaver.ControlType.stereo){
		//	target.GetComponent<MouseLook>().enabled=false;
		}

	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 
		                                         target.localEulerAngles.y, 
		                                         transform.localEulerAngles.z); 
		target.position=cameraAvator.position;
		InputListener inp=GetComponent<InputListener>();
		Vector3 direction=new Vector3(inp.Horizontal,0,inp.Vertical);
		transform.Translate(direction*speed * Time.deltaTime, Space.Self);
	}
}
