using UnityEngine;
using System.Collections;

public class MonoController : MonoBehaviour {
	public InputListener inputListener;
	public float horizontal;
	public float vertical;
	private float rotateSpeed = 250.0f;
	[SerializeField]
	Transform target;
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

		if(Input.GetMouseButtonDown(2))
			inputListener.OnKeyPress(KeyCode.B);
		if(Input.GetMouseButtonUp(2))
			inputListener.OnKeyUp(KeyCode.B);
		if(Input.GetMouseButtonDown(1))
			inputListener.OnKeyPress(KeyCode.A);
		if(Input.GetMouseButtonUp(1))
			inputListener.OnKeyUp(KeyCode.A);

	}

	void Update () 
	{
		// Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down. 
//		if(Input.GetMouseButton(0)) { 
//			transform.rotation = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0); 
//		} else { 
		 
//		target.Rotate(0,Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0); 
//		} 
	}

}
