using UnityEngine;
using System.Collections;

public class MonoController : MonoBehaviour {
	public InputListener inputListener;
	public float horizontal;
	public float vertical;
	private float rotateSpeed = 250.0f;

	// Use this for initialization
	void Start () {
		inputListener=GetComponent<InputListener>();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		inputListener.Horizontal=horizontal;
		inputListener.Vertical=vertical;

		inputListener.SetButtonValue("Fire1",Input.GetMouseButton(0));
		inputListener.SetButtonValue("Fire2",Input.GetMouseButton(1));
		inputListener.SetButtonValue("Fire3",Input.GetMouseButton(2));
		#endif
	}
	
}
