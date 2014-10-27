using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {
	public InputListener inputListener;
	public float horizontal;
	public float vertical;
	// Use this for initialization
	void Start () {
		inputListener=GetComponent<InputListener>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		inputListener.Horizontal=horizontal;
		inputListener.Vertical=vertical;
//		inputListener.ButtonBPress=Input.GetMouseButton(0);

		if(Input.GetMouseButtonDown(0))
			inputListener.OnKeyPress(KeyCode.B);
		if(Input.GetMouseButtonUp(0))
			inputListener.OnKeyUp(KeyCode.B);
		if(Input.GetMouseButtonDown(1))
			inputListener.OnKeyPress(KeyCode.A);
		if(Input.GetMouseButtonUp(1))
			inputListener.OnKeyUp(KeyCode.A);

	}


}
