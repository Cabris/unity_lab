﻿using UnityEngine;
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
	}
}