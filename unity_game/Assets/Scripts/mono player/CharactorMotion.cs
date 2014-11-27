using UnityEngine;
using System.Collections;

public class CharactorMotion : MonoBehaviour {

	[SerializeField]
	Transform target;
	[SerializeField]
	Transform cameraAvator;
	[SerializeField]
	Transform myCamera;
	// Use this for initialization
	void Start () {
		myCamera.parent=transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 
		                                         target.localEulerAngles.y, 
		                                         transform.localEulerAngles.z); 
		myCamera.position=cameraAvator.position;
		InputListener inp=GetComponent<InputListener>();
		Vector3 t=new Vector3(inp.Horizontal,0,inp.Vertical);
		transform.Translate(t*2.5f * Time.deltaTime, Space.Self);
	}
}
