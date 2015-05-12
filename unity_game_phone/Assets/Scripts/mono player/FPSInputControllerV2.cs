using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;
[RequireComponent(typeof(CharacterMotor))]
public class FPSInputControllerV2 : MonoBehaviour {
	
	private CharacterMotor motor;
	private InputListener inputListener;
	// Use this for initialization
	void Awake () {
		motor = GetComponent("CharacterMotor") as CharacterMotor;
		inputListener=GetComponent<InputListener>();
	}
	
	// Update is called once per frame
	void Update () {
		// Get the input vector from kayboard or analog stick
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		directionVector = new Vector3(inputListener.Horizontal, 0, inputListener.Vertical);
		
		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.rotation * directionVector;
		motor.inputJump = Input.GetButton("Jump");
	}
}


